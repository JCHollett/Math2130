using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Math2130Project3 {

   public class Generation {
      public int CurrentGeneration { get; private set; }
      public int Number { get; private set; }
      public Generation Previous { get; private set; }
      public Human[] Population { get; private set; }

      public Generation( Generation p , int N , bool unique ) {
         this.CurrentGeneration = 1;
         this.Previous = p;
         this.Number = N;
         this.Population = new Human[N];
         if ( Previous != null ) {
            this.CurrentGeneration = this.Previous.CurrentGeneration + 1;
            if ( !unique ) {
               for ( int i = 0; i < N; i++ ) {
                  this.Population[i] = new Human( Previous.getRandom() , Previous.getRandom() , this , i );
               }
            } else {
               for ( int i = 0; i < N; i++ ) {
                  this.Population[i] = new Human( Previous.Population , this , i );
               }
            }
         } else {
            for ( int i = 0; i < N; i++ ) {
               this.Population[i] = new Human( null , null , this , i );
            }
         }
      }

      public static int AncestorDifference( Generation origin , Generation current ) {
         List<Human> ancestors = origin.Population.ToList();
         List<Human> currentAncestors = current.UniqueAncestors();
         return ancestors.Intersect( currentAncestors ).ToList().Count;
      }

      public int TotalUnique() {
         return this.UniqueAncestors().Count;
      }

      public Human getRandom() {
         Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

         int rnd = rndNum.Next(0, Number);
         return Population[rnd];
      }

      public List<Human> UniqueAncestors() {
         List<Human> list = new List<Human>();
         bool start = true;
         foreach ( Human h in this.Population ) {
            if ( list.Count == 0 & start ) {
               list = h.Ancestors;
               start = false;
            } else {
               list = list.Intersect( h.Ancestors ).ToList();
            }
         }
         return list;
      }

      public override string ToString() {
         string x = "";
         foreach ( Human h in Population ) {
            x += "(" + this.CurrentGeneration + "," + h.ID + ")";
         }
         x = x.Replace( ")(" , "), (" );
         return "[" + x + "]";
      }
   }

   public class Human {
      public int ID;
      public Generation Current { get; private set; }
      public Human[] Ancestor;
      public List<Human> Ancestors { get; private set; }
      public Human this[int i] { get { return Ancestor[i]; } }

      public Human( Human a , Human b , Generation g , int ID ) {
         this.ID = ID;
         if ( a == null || b == null ) {
            this.Ancestor = null;
            this.Ancestors = new List<Human>();
         } else if ( a.Ancestor == null && b.Ancestor == null ) {
            this.Ancestors = new List<Human>();
            this.Ancestor = new Human[] { a , b };
            this.Ancestors = this.Ancestor.ToList();
         } else {
            this.Ancestors = new List<Human>();
            this.Ancestor = new Human[] { a , b };
            this.Ancestors = a.Ancestors.Union( b.Ancestors ).ToList();
         }
         this.Current = g;
      }

      public Human( Human[] population , Generation g , int ID ) {
         this.ID = ID;
         Human a = g.Previous.getRandom();
         Human b = a;
         Human unique = null;
         while ( a.Equals( b ) ) {
            if ( !( unique = g.Previous.getRandom() ).Equals( a ) ) {
               b = unique;
            }
         }
         if ( a == null || b == null ) {
            this.Ancestor = null;
            this.Ancestors = new List<Human>();
         } else if ( a.Ancestor == null && b.Ancestor == null ) {
            this.Ancestors = new List<Human>();
            this.Ancestor = new Human[] { a , b };
            this.Ancestors = this.Ancestor.ToList();
         } else {
            this.Ancestors = new List<Human>();
            this.Ancestor = new Human[] { a , b };

            this.Ancestors = a.Ancestors.Union( b.Ancestors ).ToList();
         }
         this.Current = g;
      }

      public override string ToString() {
         if ( this.Ancestor != null && this.Ancestor != null ) {
            return "[" + "(" + this.Current.CurrentGeneration + "," + this.ID + ")=>(" + this.Ancestor[0].Current.CurrentGeneration + "," + this.Ancestor[0].ID + "), (" + this.Ancestor[1].Current.CurrentGeneration + "," + this.Ancestor[1].ID + ")]";
         } else {
            return "[" + "(" + this.Current.CurrentGeneration + "," + this.ID + ")]";
         }
      }
   }

   public class Tester {

      public static void Basic() {
         Generation g = new Generation(null, 5 ,false);
         g = new Generation( g , 5 , false );
         g = new Generation( g , 5 , false );
         g = new Generation( g , 5 , false );
         g = new Generation( g , 5 , false );
         g = new Generation( g , 5 , false );
         g = new Generation( g , 5 , false );
         Human[] humans = g.UniqueAncestors().ToArray();
         foreach ( Human h in humans ) {
            Console.Write( "|" + h + "|" );
            Console.WriteLine();
         }
      }

      public static double[] Iterative( int n , int gens , double percent , bool unique ) {
         double[] total = new double[gens];
         Generation g = new Generation(null, n, unique);
         Generation origin = g;
         if ( n > 0 ) {
            g = new Generation( g , n = ( int )( Math.Ceiling( n * ( 1 + percent ) ) ) , unique );
            for ( int i = 1; i < gens; i++ ) {
               total[i - 1] = g.TotalUnique();
               g = new Generation( g , n = ( int )( Math.Ceiling( n * ( 1 + percent ) ) ) , unique );
            }
            total[gens - 1] = g.TotalUnique();
         }
         return total;
      }

      public static void Test( int N , int InitialPop , double percent , int G , bool unique ) {
         double[] avg = new double[G];
         for ( int i = 0; i < N; i++ ) {
            double[] result = Iterative( InitialPop , G , percent , unique );
            avg = avg.Zip( result , ( x , y ) => x + y / N ).ToArray();
         }
         string path = @"N="+N+",P="+InitialPop+"@"+(percent+1)+",G="+G+",U="+unique+".txt";
         StreamWriter file = null;
         if ( File.Exists( path ) ) {
            File.Delete( path );
            file = File.CreateText( path );
         } else {
            file = File.CreateText( path );
         }
         for ( int i = 0; i < G; i++ ) {
            file.WriteLine( "{0:0.000} {1:0.000}" , i + 1 , avg[i] );
         };
         file.Close();
      }

      public static void ProveRandom() {
         StreamWriter file = null;
         string path = @"RandomPlot25.txt";

         if ( File.Exists( path ) ) {
            File.Delete( path );
            file = File.CreateText( path );
         } else {
            file = File.CreateText( path );
         }
         for ( int i = 1; i <= 1000; i++ ) {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            double rnd = rndNum.Next(0,25);

            file.WriteLine( "{0} {1}" , i , rnd );
         }

         file.Close();
      }

      public static void Main( string[] args ) {
         //Test( 250 , 10 , 0 , 10 , false );
         //Test( 250 , 10 , 0 , 50 , false );
         //Test( 250 , 25 , 0 , 50 , false );
         //Test( 250 , 50 , 0 , 50 , false );
         //Test( 250 , 100 , 0 , 50 , false );
         //Test( 250 , 200 , 0 , 50 , false );
         //Test( 250 , 300 , 0 , 50 , false );
         //Test( 250 , 10 , 0 , 50 , true );
         //Test( 250 , 25 , 0 , 50 , true );
         //Test( 250 , 50 , 0 , 50 , true );
         //Test( 250 , 100 , 0 , 50 , true );
         //Test( 250 , 200 , 0 , 50 , true );
         //Test( 250 , 300 , 0 , 50 , true );
         //Test( 250 , 25 , 0.05 , 25 , false );
         //Test( 250 , 25 , 0.10 , 25 , false );
         //Test( 250 , 25 , 0.25 , 25 , false );

         //Test( 250 , 25 , 0.05 , 25 , true );
         //Test( 250 , 25 , 0.10 , 25 , true );
         //Test( 250 , 25 , 0.25 , 25 , true );
         //Test( 250 , 10 , 0.0 , 20 , false );
      }
   }
}