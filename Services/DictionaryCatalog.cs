using DicSign.Models;

namespace DicSign.Services;

public static class DictionaryCatalog
{
    public static readonly IReadOnlyList<DictionaryEntry> Entries = BuildEntries();

    private static List<DictionaryEntry> BuildEntries()
    {
        var entries = new List<DictionaryEntry>
        {
            new() { Id = "hello", Name = "Hola", Category = "Saludos", Description = "Expresión que se utiliza para saludar o iniciar una conversación." },
            new() { Id = "thanks", Name = "Gracias", Category = "Saludos", Description = "Expresión para agradecer a otra persona." },
            new() { Id = "please", Name = "Por favor", Category = "Saludos", Description = "Expresión amable que acompaña una petición." },
            new() { Id = "goodbye", Name = "Adiós", Category = "Saludos", Description = "Expresión que se utiliza al despedirse." },
            new() { Id = "goodmorning", Name = "Buenos días", Category = "Saludos", Description = "Saludo que se utiliza durante la mañana." },
            new() { Id = "goodnight", Name = "Buenas noches", Category = "Saludos", Description = "Saludo o despedida que se utiliza por la noche." },
            new() { Id = "sorry", Name = "Perdón", Category = "Saludos", Description = "Expresión para disculparse o pedir que se excuse una acción." },
            new() { Id = "yes", Name = "Sí", Category = "Saludos", Description = "Respuesta afirmativa." },
            new() { Id = "no", Name = "No", Category = "Saludos", Description = "Respuesta negativa." },
        };

        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        foreach (char letter in alphabet)
        {
            entries.Add(new DictionaryEntry
            {
                Id = $"letter-{letter}",
                Name = letter.ToString(),
                Category = "Abecedario",
                Description = $"Letra {letter} del alfabeto. Consulta el recurso educativo para ver su configuración manual en LSM.",
                IsLetter = true
            });
        }

        return entries;
    }

    public static IReadOnlyList<DictionaryEntry> Alphabet => Entries.Where(x => x.IsLetter).ToList();
}
