using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParanormalAgency.Models
{
    public class Investigation : IValidatableObject
    {
        private int id;
        private string caseName = "";
        private string clientName = "";
        private string phenomenonType = "";
        private string riskLevel = "";
        private decimal? baseBudget;
        private bool? requiresSpecializedEquipment;
        private DateTime? investigationDate;
        private decimal totalCost;

        public static IReadOnlyList<string> PhenomenonTypes { get; } = Array.AsReadOnly(new[] { "Aparición", "Poltergeist", "Actividad demoníaca", "Objeto maldito" });
        public static IReadOnlyList<string> RiskLevels { get; } = Array.AsReadOnly(new[] { "Bajo", "Medio", "Alto", "Crítico" });

        // EF6 y el model binding requieren un constructor sin parámetros.
        public Investigation() { }

        public Investigation(string caseName, string clientName, string phenomenonType, string riskLevel, decimal baseBudget, bool requiresSpecializedEquipment, DateTime investigationDate)
        {
            CaseName = caseName;
            ClientName = clientName;
            PhenomenonType = phenomenonType;
            RiskLevel = riskLevel;
            BaseBudget = baseBudget;
            RequiresSpecializedEquipment = requiresSpecializedEquipment;
            InvestigationDate = investigationDate;
            CalculateTotalCost();
        }

        [Key]
        public int Id { get => id; set => id = value; }

        [Required(ErrorMessage = "Ingrese el nombre del caso.")]
        [StringLength(60, ErrorMessage = "El nombre del caso admite máximo 50 caracteres.")]
        [RegularExpression(@"^[\p{L}\p{M}0-9]+(?:[ \-]*[\p{L}\p{M}0-9]+)*$", ErrorMessage = "Use letras, números, espacios o guiones en el nombre del caso.")]
        [Display(Name = "Nombre del caso")]
        public string CaseName { get => caseName; set => caseName = value; }

        [Required(ErrorMessage = "Ingrese el nombre del cliente.")]
        [StringLength(80, ErrorMessage = "El nombre del cliente admite máximo 80 caracteres.")]
        [RegularExpression(@"^[\p{L}\p{M}]+(?: [\p{L}\p{M}]+)*$", ErrorMessage = "Use solo letras y espacios en el nombre del cliente.")]
        [Display(Name = "Nombre del cliente")]
        public string ClientName { get => clientName; set => clientName = value; }

        [Required(ErrorMessage = "Seleccione un fenómeno.")]
        [Display(Name = "Tipo de fenómeno")]
        public string PhenomenonType { get => phenomenonType; set => phenomenonType = value; }

        [Required(ErrorMessage = "Seleccione un nivel de riesgo.")]
        [Display(Name = "Nivel de riesgo")]
        public string RiskLevel { get => riskLevel; set => riskLevel = value; }

        [Required(ErrorMessage = "Ingrese el presupuesto base.")]
        [Range(typeof(decimal), "0", "999999999999.99", ErrorMessage = "Ingrese un presupuesto válido en USD.")]
        [Display(Name = "Presupuesto base (USD)")]
        public decimal? BaseBudget { get => baseBudget; set => baseBudget = value; }

        [Required(ErrorMessage = "Indique si requiere equipo especializado.")]
        [Display(Name = "Requiere equipo especializado")]
        public bool? RequiresSpecializedEquipment { get => requiresSpecializedEquipment; set => requiresSpecializedEquipment = value; }

        [Required(ErrorMessage = "Seleccione la fecha de investigación.")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Fecha de investigación")]
        public DateTime? InvestigationDate { get => investigationDate; set => investigationDate = value; }

        // El costo se calcula en el servidor; no se permite modificarlo desde formularios.
        [Display(Name = "Costo total (USD)")]
        public decimal TotalCost { get => totalCost; private set => totalCost = value; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (!PhenomenonTypes.Contains(PhenomenonType))
                yield return new ValidationResult("Seleccione un fenómeno válido.", new[] { nameof(PhenomenonType) });
            if (!RiskLevels.Contains(RiskLevel))
                yield return new ValidationResult("Seleccione un nivel de riesgo válido.", new[] { nameof(RiskLevel) });
            if (CaseName?.Length > 50)
                yield return new ValidationResult("El nombre del caso admite máximo 50 caracteres.", new[] { nameof(CaseName) });
            if (string.IsNullOrWhiteSpace(CaseName))
                yield return new ValidationResult("Ingrese el nombre del caso.", new[] { nameof(CaseName) });
            if (string.IsNullOrWhiteSpace(ClientName))
                yield return new ValidationResult("Ingrese el nombre del cliente.", new[] { nameof(ClientName) });
            if (InvestigationDate.HasValue && InvestigationDate.Value.TimeOfDay != TimeSpan.Zero)
                yield return new ValidationResult("La fecha no debe incluir hora.", new[] { nameof(InvestigationDate) });
            if (BaseBudget.HasValue && decimal.Round(BaseBudget.Value, 2) != BaseBudget.Value)
                yield return new ValidationResult("El presupuesto admite hasta dos decimales.", new[] { nameof(BaseBudget) });
        }

        // Todos los incrementos porcentuales se calculan sobre el presupuesto original.
        public decimal CalculateTotalCost()
        {
            if (!BaseBudget.HasValue || BaseBudget.Value < 0 || !RequiresSpecializedEquipment.HasValue)
                throw new ValidationException("Complete el presupuesto y la selección de equipo.");
            decimal phenomenonRate = PhenomenonType switch
            {
                "Aparición" => 0.15m,
                "Poltergeist" => 0.25m,
                "Actividad demoníaca" => 0.45m,
                "Objeto maldito" => 0.30m,
                _ => throw new ValidationException("Seleccione un fenómeno válido.")
            };
            decimal riskAmount = RiskLevel switch
            {
                "Bajo" => 500m,
                "Medio" => 1000m,
                "Alto" => 2000m,
                "Crítico" => 4000m,
                _ => throw new ValidationException("Seleccione un nivel de riesgo válido.")
            };
            decimal budget = BaseBudget.Value;
            TotalCost = decimal.Round(budget + budget * phenomenonRate + riskAmount +
                (RequiresSpecializedEquipment.Value ? budget * 0.20m : 0m), 2, MidpointRounding.AwayFromZero);
            return TotalCost;
        }
    }
}