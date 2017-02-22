using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Math2130Project4 {

   public enum Color {
      Black = 0, Red, Orange, Yellow, Lime, Turquoise
   }

   public class Tester {

      public static void RunTest( int Cars , int Roads , double Pr , bool ND , int N ) {
         try {
            var t = new Track(Cars, Roads, 5, Pr, ND);
            if ( File.Exists( "test" + N + ".txt" ) ) {
               File.Delete( "test" + N + ".txt" );
            }
            if ( File.Exists( "color" + N + ".txt" ) ) {
               File.Delete( "color" + N + ".txt" );
            }

            var file = File.CreateText( "test" + N + ".txt" );
            var file2 = File.CreateText( "color" + N + ".txt" );
            var ParallelQuery = t.Cars.AsParallel();
            for ( int i = 0; i <= 10000; i++ ) {
               ParallelQuery.ForAll( car => car.Calculate() );
               ParallelQuery.ForAll( car => car.Move() );
            }
            for ( int i = 0; i <= 1000; i++ ) {
			/**
               if ( i == 200 ) {
                  t.Cars[ 0 ].VMax = 0;
                  t.Cars[ 0 ].V = 0;
               }
               if ( i == 220 ) {
                  t.Cars[ 0 ].VMax = 5;
               }
			**/
               ParallelQuery.ForAll( car => car.Calculate() );
               ParallelQuery.ForAll( car => car.Move() );
               var query = t.Cars.AsParallel();
               var T1 = new Task( () => {
                  query.ToList().ForEach( a => {
                     file.WriteLine( "{0} {1}" , a.Location.Index + 1 , i );
                  } );
               } );
               var T2 = new Task( () => {
                  query.ToList().ForEach( a => {
                     file2.WriteLine( "{0}" , Enum.GetName( typeof( Color ) , a.V ) );
                  } );
               } );
               T1.Start();
               T2.Start();
               T2.Wait();
               T1.Wait();
            }
            file.Close();
            file2.Close();
            Console.WriteLine( "Test #{0} success; continue" , N );
         } catch ( IOException ) {
            Console.WriteLine( "Test #{0} read failure; continue" , N );
         }
      }

      public static void Main( string[ ] args ) {
         DateTime before = DateTime.Now;
         List<Task> tasks = new List<Task>();
         //tasks.Add( new Task( () => RunTest( 200 , 1000 , 0.05 , false , 1 ) ) );
         //tasks.Add( new Task( () => RunTest( 300 , 1000 , 0.05 , false , 2 ) ) );
         //tasks.Add( new Task( () => RunTest( 400 , 1000 , 0.05 , false , 3 ) ) );
         //tasks.Add( new Task( () => RunTest( 100 , 1000 , 0.15 , false , 4 ) ) );
         //tasks.Add( new Task( () => RunTest( 100 , 1000 , 0.25 , false , 5 ) ) );
         //tasks.Add( new Task( () => RunTest( 100 , 1000 , 0.50 , false , 6 ) ) );
         //tasks.Add( new Task( () => RunTest( 200 , 1000 , 0.10 , true , 7 ) ) );
         tasks.Add( new Task( () => RunTest( 200 , 1000 , 0.10 , false , 8 ) ) );
         foreach ( Task t in tasks ) {
            t.Start();
         }
         foreach ( Task t in tasks ) {
            t.Wait();
         }
         DateTime after = DateTime.Now;
         TimeSpan dif = after - before;
         Console.WriteLine( "Completed in: {0}" , dif );
         Console.WriteLine( "Done" );
         Console.ReadKey();
      }
   }
}