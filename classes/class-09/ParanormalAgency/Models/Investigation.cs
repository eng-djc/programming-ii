using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParanormalAgency.Models
{
    // Representa una investigación paranormal y calcula su costo.
    public class Investigation : IValidatableObject
    {
        // Identificador entero que distingue cada investigación en la base de datos.
        private int id;
        // Los textos comienzan vacíos y se completan desde el formulario.
        private string caseName = "";
        private string clientName = "";
        private string phenomenonType = "";
        private string riskLevel = "";
        // decimal? permite detectar un presupuesto sin ingresar y conservar precisión monetaria.
        private decimal? baseBudget;
        // bool? diferencia Sí, No y la ausencia de una selección.
        private bool? requiresSpecializedEquipment;
        // DateTime? permite detectar una fecha pendiente; la validación descarta horas.
        private DateTime? investigationDate;
        // decimal almacena el importe final calculado, sin admitir valores nulos.
        private decimal totalCost;

        public static IReadOnlyList<string> PhenomenonTypes { get; } = Array.AsReadOnly(new[] { "Aparición", "Poltergeist", "Actividad demoníaca", "Objeto maldito" });
        public static IReadOnlyList<string> RiskLevels { get; } = Array.AsReadOnly(new[] { "Bajo", "Medio", "Alto", "Crítico" });

        // Constructor vacío para cargar registros y enlazar datos del formulario.
        public Investigation() { }

        // Inicializa una investigación con sus datos y calcula el costo inicial.
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
        [StringLength(60, ErrorMessage = "El nombre del caso admite máximo 60 caracteres.")]
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

        // Conserva el costo calculado sin permitir su edición directa.
        [Display(Name = "Costo total (USD)")]
        public decimal TotalCost { get => totalCost; private set => totalCost = value; }

        // Rechaza fenómenos o riesgos fuera de las opciones disponibles, casos de más de 50 caracteres,
        // nombres vacíos, fechas con hora y presupuestos con más de dos decimales.
        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            // Comprueba que las categorías correspondan a las opciones definidas por el sistema.
            if (!PhenomenonTypes.Contains(PhenomenonType))
                yield return new ValidationResult("Seleccione un fenómeno válido.", new[] { nameof(PhenomenonType) });
            if (!RiskLevels.Contains(RiskLevel))
                yield return new ValidationResult("Seleccione un nivel de riesgo válido.", new[] { nameof(RiskLevel) });
            // Limita el nombre del caso a 50 caracteres y rechaza nombres vacíos.
            if (CaseName?.Length > 50)
                yield return new ValidationResult("El nombre del caso admite máximo 50 caracteres.", new[] { nameof(CaseName) });
            if (string.IsNullOrWhiteSpace(CaseName))
                yield return new ValidationResult("Ingrese el nombre del caso.", new[] { nameof(CaseName) });
            if (string.IsNullOrWhiteSpace(ClientName))
                yield return new ValidationResult("Ingrese el nombre del cliente.", new[] { nameof(ClientName) });
            // Una fecha ingresada debe representar solo el día, sin componente horario.
            if (InvestigationDate.HasValue && InvestigationDate.Value.TimeOfDay != TimeSpan.Zero)
                yield return new ValidationResult("La fecha no debe incluir hora.", new[] { nameof(InvestigationDate) });
            // El presupuesto monetario admite como máximo dos posiciones decimales.
            if (BaseBudget.HasValue && decimal.Round(BaseBudget.Value, 2) != BaseBudget.Value)
                yield return new ValidationResult("El presupuesto admite hasta dos decimales.", new[] { nameof(BaseBudget) });
        }

        // Calcula el costo sumando presupuesto, fenómeno, riesgo y equipo especializado.
        // Ambos porcentajes se aplican al presupuesto base.
        public decimal CalculateTotalCost()
        {
            if (!BaseBudget.HasValue || BaseBudget.Value < 0 || !RequiresSpecializedEquipment.HasValue)
                throw new ValidationException("Complete el presupuesto y la selección de equipo.");
            // Asigna el porcentaje de recargo correspondiente al fenómeno.
            decimal phenomenonRate = PhenomenonType switch
            {
                "Aparición" => 0.15m,
                "Poltergeist" => 0.25m,
                "Actividad demoníaca" => 0.45m,
                "Objeto maldito" => 0.30m,
                _ => throw new ValidationException("Seleccione un fenómeno válido.")
            };
            // El riesgo agrega un monto fijo, no un porcentaje.
            decimal riskAmount = RiskLevel switch
            {
                "Bajo" => 500m,
                "Medio" => 1000m,
                "Alto" => 2000m,
                "Crítico" => 4000m,
                _ => throw new ValidationException("Seleccione un nivel de riesgo válido.")
            };
            // Los dos recargos porcentuales se calculan sobre el mismo presupuesto original.
            decimal budget = BaseBudget.Value;
            decimal phenomenonCost = budget * phenomenonRate;
            decimal equipmentCost = RequiresSpecializedEquipment.Value ? budget * 0.20m : 0m;
            decimal total = budget + phenomenonCost + riskAmount + equipmentCost;
            // Guarda el total con dos decimales y devuelve el importe calculado.
            TotalCost = decimal.Round(total, 2, MidpointRounding.AwayFromZero);
            return TotalCost;
        }
    }
}