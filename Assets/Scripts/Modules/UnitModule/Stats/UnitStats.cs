namespace UnitModule
{
    public class UnitStats
    {
        private Stats _stats;
        
        public void Add(Stats statToAdd)
        {
            _stats |= statToAdd;
        }

        public void Remove(Stats statToRemove)
        {
            _stats &= ~statToRemove;
        }

        public bool Has(Stats statToCheck)
        {
            return _stats.HasFlag(statToCheck);
        }
    }
}