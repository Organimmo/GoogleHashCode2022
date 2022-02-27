namespace HashCode2022
{

    public class Skill
    {
        public string Name { get; }
        public int Level { get; private set; }

        public Skill(string name, int level = 0)
        {
            Name = name;
            Level = level;
        }

        public void IncrementLevel()
        {
            Level++;
        }
    }

}
