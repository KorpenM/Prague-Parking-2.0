using System;
using System.Collections.Generic;
using Prague_Parking_2._0;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using System.Xml;
using PragueParking_2;
using PragueParking_2._0;


internal class Program
{
    private static Garage garage = new Garage();
    private static UI ui = new(garage);

    private static void Main()
    {
        garage.LoadSettings();
    }
}