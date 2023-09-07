using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.SOLID
{
    public enum Material
    {
        Wood, Metal, Plastic
    }

    public enum Type
    {
        Wind, Chord, Percussion
    }

    public interface ISpecification<T>
    {
        bool IsSatisfied(T item);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class Instrument
    {
        public string Name;
        public Type Type;
        public Material Material;

        public Instrument(string name, Type type, Material material)
        {
            this.Name = name;
            this.Type = type;
            this.Material = material;
        }
    }


    /*
     * Instead of having to change the filter implementation,
     * you just add new spec implementations and use them with the filter
     */
    public class MaterialSpecification : ISpecification<Instrument>
    {
        private Material material;

        public MaterialSpecification(Material material)
        {
            this.material = material;
        }

        public bool IsSatisfied(Instrument item)
        {
            return item.Material == material;
        }
    }

    public class TypeSpecification : ISpecification<Instrument>
    {
        private Type type;

        public TypeSpecification(Type type)
        {
            this.type = type;
        }

        public bool IsSatisfied(Instrument item)
        {
            return item.Type == type;
        }
    }

    public class AndSpecification : ISpecification<Instrument>
    {
        List<ISpecification<Instrument>> specifications;

        public AndSpecification(List<ISpecification<Instrument>> specifications)
        {
            this.specifications = specifications;
        }

        public bool IsSatisfied(Instrument item)
        {
            foreach (var spec in this.specifications)
                if (!spec.IsSatisfied(item))
                    return false;

            return true;
        }
    }

    /*
     * It goes thru the items and return the ones who satisfied the spec
     */
    public class InstrumentFilter : IFilter<Instrument>
    {
        public IEnumerable<Instrument> Filter(IEnumerable<Instrument> items, ISpecification<Instrument> spec)
        {
            foreach (var item in items)
                if (spec.IsSatisfied(item))
                    yield return item;
        }
    }

    // This way, you can easily use the filter as an extended method of a product list
    public static class InstrumentExtensions
    {
        static InstrumentFilter instrumentFilter;

        static InstrumentExtensions()
        {
            instrumentFilter = new InstrumentFilter();
        }

        public static IEnumerable<Instrument> Filter(this IEnumerable<Instrument> items, ISpecification<Instrument> spec)
        {
            return instrumentFilter.Filter(items, spec);
        }
    }

    public static class SpecificationPattern
    {
        public static void Execute()
        {
            List<Instrument> instruments = new List<Instrument>()
            {
                new Instrument("Flute", Type.Wind, Material.Metal),
                new Instrument("Piano", Type.Chord, Material.Wood),
                new Instrument("Drums", Type.Percussion, Material.Metal),
            };

            MaterialSpecification metalSpec = new MaterialSpecification(Material.Metal); 
            TypeSpecification windSpec = new TypeSpecification(Type.Wind);
            AndSpecification metalAndWindSpec = new AndSpecification(new List<ISpecification<Instrument>>() {
                metalSpec, 
                windSpec
            });

            Console.WriteLine("Filter metal instruments :");
            foreach (var item in instruments.Filter(metalSpec))
            {
                Console.WriteLine($" - {item.Name}");
            }

            Console.WriteLine("\nFilter metal and wind instruments :");
            foreach (var item in instruments.Filter(metalAndWindSpec))
            {
                Console.WriteLine($" - {item.Name}");
            }
        }
    }
}
