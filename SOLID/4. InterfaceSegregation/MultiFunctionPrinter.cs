using System;

namespace DesignPatterns.SOLID;

public class Document
{
}

public interface IMachine
{
    void Print(Document d);
    void Fax(Document d);
    void Scan(Document d);
}

// ok if you need a multifunction machine
public class MultiFunctionPrinterBad : IMachine
{
    public void Print(Document d)
    {
        //
    }

    public void Fax(Document d)
    {
        //
    }

    public void Scan(Document d)
    {
        //
    }
}

/*
 * Example of anti-pattern
 * lots of unimplemented methods due to lack of interface segregation
 */
public class OldFashionedPrinter : IMachine
{
    public void Print(Document d)
    {
        // yep
    }

    public void Fax(Document d)
    {
        throw new System.NotImplementedException();
    }

    public void Scan(Document d)
    {
        throw new System.NotImplementedException();
    }
}

/*
 * Better to have specialized interfaces
 */
public interface IPrinter
{
    void Print(Document d);
}

public interface IScanner
{
    void Scan(Document d);
}

public class Printer : IPrinter
{
    public void Print(Document d)
    {

    }
}

/*
 * You can accomplish a multi purpose machine by implemented the specialized interfaces
 */
public class Photocopier : IPrinter, IScanner
{
    public void Print(Document d)
    {
        throw new System.NotImplementedException();
    }

    public void Scan(Document d)
    {
        throw new System.NotImplementedException();
    }
}

/*
 * You can accomplish a multi purpose interface by inheriting the specialized interfaces
 */
public interface IMultiFunctionDevice : IPrinter, IScanner //
{

}

/*
 * You can use already existing implementations of specialized interfaces and used them
 * for classes that need them
 */
public struct MultiFunctionPrinter : IMultiFunctionDevice
{
    // compose this out of several modules
    private IPrinter printer;
    private IScanner scanner;

    public MultiFunctionPrinter(IPrinter printer, IScanner scanner)
    {
        if (printer == null)
        {
            throw new ArgumentNullException(paramName: nameof(printer));
        }
        if (scanner == null)
        {
            throw new ArgumentNullException(paramName: nameof(scanner));
        }
        this.printer = printer;
        this.scanner = scanner;
    }

    public void Print(Document d)
    {
        printer.Print(d);
    }

    public void Scan(Document d)
    {
        scanner.Scan(d);
    }
}
