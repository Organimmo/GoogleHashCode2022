using HashCode2022.Strategies;
using System.Collections.Generic;
using System.Linq;

namespace HashCode2022.Drivers
{
    internal class FixedDriver : Driver
    {
        private readonly static StrategySet[] strategies = {
            // new StrategySet(new ContributorStrategyA(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyD(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyE(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyG(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyA(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyD(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyE(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyG(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyG(false)),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyG()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyG()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyG()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyH()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyH()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyH()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyH()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyI()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyI()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyI()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyI()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyJ()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyJ()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyJ()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyJ()),

            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyK()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyK()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyK()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyK()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyL()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyL()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyL()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyL()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyM()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyM()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyM()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyM()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyN()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyN()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyN()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyN()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -4.0)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -7)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -8)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -9)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -10)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -1)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -0.5)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 0.5)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 1)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -10)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -5)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 5)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 10)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -100)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -50)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 50)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 100)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -80)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -60)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -40)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -20)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -45)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -55)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -65)),
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -75)), // best for file "a"/"d"
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -67)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -70)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -72)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -77)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -73)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -74)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -76)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -78)),
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -4.2)), // best for "e"
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(0.12, 11.68, -24)), // best for file "b"
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(6.24, -12.16, -114)), // best for file "c"
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(-1.6, 4.96, -6)),
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(5.56, -13.04, -108)), // best for file "f"
        };

        public FixedDriver(string inputPath, string outputPath, int fileNo = -1)
        : base(inputPath, outputPath, fileNo)
        {
        }

        protected override IEnumerable<StrategySet> GetStrategySets()
        {
            return strategies.AsEnumerable();

            // int count = 4;
            // double start = -7.0;
            // double stop = -6.0;

            // // Try to minimize number cluttering
            // double stepRaw = (stop - start) / (count + 1);
            // int digits = -(int)Math.Round(Math.Log10(stepRaw));
            // double stepIncrement = Math.Pow(10, -digits);
            // double step = Math.Floor(stepRaw / stepIncrement) * stepIncrement;
            // double middle = Math.Round((start + stop) / 2 / stepIncrement) * stepIncrement;

            // double first;
            // if (count % 2 == 1) // e.g count = 5, start/stop/step = 3/4/0.2 --> middle = 3.5 --> first = 3.1
            //     first = middle - (count / 2) * step;
            // else // e.g count = 4, start/stop/step = 3/4/0.2 --> middle = 3.5 --> first = 3.1
            //     first = middle - step / 2 - ((count - 1) / 2) * step;

            // for (int i = 0; i < count; i++)
            // {
            //     double number = Math.Round(first + i * step, digits);
            //     yield return new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, number));
            // }
        }
    }
}
