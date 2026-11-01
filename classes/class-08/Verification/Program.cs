using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cherkessia.Models;

int checks = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new Exception("FAIL: " + message);
    checks++;
}
bool Valid(object value) => Validator.TryValidateObject(value, new ValidationContext(value), new List<ValidationResult>(), true);
DateTime today = new DateTime(2026, 10, 31);
Instrumento Instrument(int age) => new Instrumento(1, "Sensor", today.AddYears(-age), 10.50m, 5);
int[] points = { 3, 3, 3, 2, 2, 1, 0, 0 };
for (int age = 0; age < points.Length; age++)
{
    var instrument = Instrument(age);
    Check(instrument.calcularAntiguedad(today) == age, "Age " + age);
    Check(instrument.calcularPuntos(today) == points[age], "Points " + age);
    Check(instrument.calcularGarantia(today) == (age <= 5), "Warranty " + age);
    foreach (bool active in new[] { false, true })
    {
        for (int gas = 0; gas < 3; gas++)
        {
            var volcano = new Volcan("Irazú", "Cartago", active, Volcan.OpcionesGases[gas], 1);
            int expected = (active ? 2 : 0) + gas + 1 + points[age];
            Check(volcano.calcularIRV(instrument, today) == expected, "IRV combination");
            string risk = expected <= 2 ? "Riesgo Muy Bajo" : expected <= 4 ? "Riesgo Bajo" : expected <= 6 ? "Riesgo Medio" : "Riesgo Alto";
            Check(volcano.clasificarRiesgo() == risk, "Risk combination");
        }
    }
}
var anniversary = new Instrumento(2, "Sensor", new DateTime(2021, 11, 1), 0m, 5);
Check(anniversary.calcularAntiguedad(today) == 4, "Day before anniversary");
Check(anniversary.calcularAntiguedad(today.AddDays(1)) == 5, "Anniversary");
var leap = new Instrumento(3, "Sensor", new DateTime(2024, 2, 29), 0m, 1);
Check(leap.calcularAntiguedad(new DateTime(2025, 2, 27)) == 0, "Before leap anniversary");
Check(leap.calcularAntiguedad(new DateTime(2025, 2, 28)) == 1, "Leap anniversary convention");
Check(Valid(anniversary), "Valid instrument including zero price");
foreach (int warranty in new[] { 0, 6 }) { var x = Instrument(1); x.Garantia = warranty; Check(!Valid(x), "Invalid warranty"); }
foreach (int warranty in new[] { 1, 5 }) { var x = Instrument(1); x.Garantia = warranty; Check(Valid(x), "Boundary warranty"); }
foreach (decimal price in new[] { -1m, 1.001m, 10000000000000000m }) { var x = Instrument(1); x.Precio = price; Check(!Valid(x), "Invalid price"); }
var missing = Instrument(1); missing.FechaRegistro = null; Check(!Valid(missing), "Required date");
missing = Instrument(1); missing.Precio = null; Check(!Valid(missing), "Required price");
missing = Instrument(1); missing.FechaRegistro = DateTime.Today.AddDays(1); Check(!Valid(missing), "Future date");
missing = Instrument(1); missing.FechaRegistro = DateTime.Today.AddHours(1); Check(!Valid(missing), "Time in date");
missing = Instrument(1); missing.Nombre = " "; Check(!Valid(missing), "Blank name");
missing = Instrument(1); missing.Codigo = 0; Check(!Valid(missing), "Zero code");
missing = Instrument(1); missing.Nombre = new string('a', 101); Check(!Valid(missing), "Name length");
foreach (string province in Volcan.Provincias)
    Check(Valid(new Volcan("Irazú", province, false, "Probable", 1)), "Province " + province);
var v = new Volcan("Irazú", "Otro", false, "Probable", 1); Check(!Valid(v), "Invalid province");
v = new Volcan("Irazú", "Cartago", false, "otro", 1); Check(!Valid(v), "Invalid gases");
v = new Volcan("Irazú", "Cartago", false, "Probable", 1); v.Activo = null; Check(!Valid(v), "Required active");
v = new Volcan("Irazú ", "Cartago", false, "Probable", 1); Check(!Valid(v), "Key whitespace");
v = new Volcan("Irazú", "Cartago", false, "Probable", 0); Check(!Valid(v), "Missing FK selection");
Check(Valid(new Volcan("Volcán / prueba", "Cartago", false, "Probable", 1)), "Names support query string characters");
Console.WriteLine($"PASS: {checks} domain checks (no database).");
