using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace WindowsFormsControlLibrary1
{
    public partial class UserControl1 : UserControl
    {
        List<Color> colorList = new List<Color>() { Color.Green, Color.Red, Color.Yellow, Color.Purple, Color.Blue };
        int num_supplier;
        int num_customers;

        public class Supplier
        {
            public int x { get; set; }
            public int y { get; set; }
            public double relativeDist { get; set; }
            public Color color { get; set; }

        }

       
        public UserControl1()
        {
            InitializeComponent();  
        }

        public bool ErrorCheck()
        {
            int value;
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MessageBox.Show("The textbox cannot be empty");
                return false;
            }
            if (!int.TryParse(textBox1.Text, out value) || !int.TryParse(textBox2.Text, out value))
            {
                MessageBox.Show("Please insert a number");
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
           if(ErrorCheck())
            {
                num_customers = Int32.Parse(textBox2.Text);
                num_supplier = Int32.Parse(textBox1.Text);
                if(num_customers<=0 || num_customers > 100 || num_supplier <=0 || num_supplier > 5)
                {
                    MessageBox.Show("The number of customers must be between 1-100 and number of suppliers must be between 1-5");
                }
                else
                {
                    List<Supplier> supplierList = new List<Supplier>();
                    List<int> possibleX = new List<int>();
                    List<int> possibleY = new List<int>();
                    
                    //Add a supplier and customer series in the chart
                    chart1.Series.Clear();
                    chart1.Series.Add("Supply").ChartType = SeriesChartType.Point;
                    chart1.Series.Add("Customer").ChartType = SeriesChartType.Point;
                    Random rand = new Random();

                    int l = 0;


                    for (int i = 0; i < num_supplier; i++)
                    {
                        Supplier supplier = new Supplier();
                        //Make sure random does not select same X or Y positions
                        do
                        {
                            supplier.x = rand.Next(-100, 500);
                            supplier.y = rand.Next(-100, 500);
                        } while (possibleX.Contains(supplier.x) || possibleY.Contains(supplier.y));
                        
                        possibleX.Add(supplier.x);
                        possibleY.Add(supplier.y);
                        supplier.color = colorList[i];
                        supplierList.Add(supplier);

                        //The supplier is plotted in the chart based on x,y position
                        chart1.Series["Supply"].Points.AddXY(supplier.x, supplier.y);

                    }


                    for (int i = 0; i < num_customers; i++)
                    {
                        int x; int y;
                        double lowestDist = 1000;
                        do
                        {
                            x = rand.Next(-100, 500);
                            y = rand.Next(-100, 500);
                        } while (possibleX.Contains(x) || possibleY.Contains(y));
                        possibleX.Add(x);
                        possibleY.Add(y);
                       

                        //Find the lowest relative distance between a customer and a supplier among all the suppliers
                        chart1.Series["Customer"].Points.AddXY(x, y);
                        for (int z = 0; z < supplierList.Count; z++)
                        {
                            int a = supplierList[z].x - x; int b = supplierList[z].y - y;
                            double relativeDist = Math.Sqrt(a * a + b * b);
                            //The relative distance of the supplier and the customer is saved as a property in the supplier class
                            supplierList[z].relativeDist = relativeDist;

                            if (lowestDist > relativeDist)
                            {
                                lowestDist = relativeDist;
                            }

                        }
                       
                        //Gets the supplier from a list of suppliers
                        //that matches the lowest relative distance between the customer and the supplier
                       var supplier = supplierList.FindAll(s => s.relativeDist == lowestDist);
                        //Adds a line between customer and supply points
                        foreach(var supp in supplier)
                        {
                            chart1.Series.Add("line" + ++l);
                            chart1.Series["line" + l].Color = supp.color;
                            chart1.Series["line" + l].Points.Add(new DataPoint(x, y));
                            chart1.Series["line" + l].Points.Add(new DataPoint(supp.x, supp.y));
                            chart1.Series["line" + l].ChartType = SeriesChartType.Line;
                        }
                       

                    }
                }
               
            }
           
        }
        

    }
}
