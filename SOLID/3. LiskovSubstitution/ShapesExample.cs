using System;

namespace DesignPatterns.SOLID;

// using a classic example
public class Rectangle
{
    //public int Width { get; set; }
    //public int Height { get; set; }

    /*
     * by setting properties as virtual you can then change
     * the referenced values of all subtypes
     */
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public Rectangle()
    {

    }

    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
    }
}

public class Square : Rectangle
{
    //public new int Width
    //{
    //  set { base.Width = base.Height = value; }
    //}

    //public new int Height
    //{ 
    //  set { base.Width = base.Height = value; }
    //}


    /*
     * By overriding virtual props, you can now change
     * subtype instances to base instances without losing references to the subtype
     */
    public override int Width // nasty side effects
    {
        set { base.Width = base.Height = value; }
    }

    public override int Height
    {
        set { base.Width = base.Height = value; }
    }
}

internal class ShapesExample
{
    static int Area(Rectangle r) => r.Width * r.Height;

    public static void Execute()
    {
        Rectangle rc = new Rectangle(2, 3);
        Console.WriteLine($"{rc} has area {Area(rc)}");

        // should be able to substitute a base type for a subtype
        /*Square*/
        Rectangle sq = new Square();
        sq.Width = 4;
        Console.WriteLine($"{sq} has area {Area(sq)}");
    }
}
