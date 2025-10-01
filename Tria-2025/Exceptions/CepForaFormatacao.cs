namespace Tria_2025.Exceptions
{
    public class CepForaFormatacao : Exception
    {
        public CepForaFormatacao(): base("CEP deve estar no padrão xxxxxxxx") { }
    }
}
