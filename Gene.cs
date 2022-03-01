namespace SS_API
{
    public class Gene
    {
        public string? Symbol { get; set; }
        public string? Function { get; set; }
        public string? Acid { get; set; }

        public Gene(string? symbol, string? function, string? acid)
        {
            Symbol = symbol;
            Function = function;
            Acid = acid;
        }

    }
}
