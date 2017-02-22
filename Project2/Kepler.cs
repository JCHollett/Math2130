using System;
using System.IO;

namespace Math2130Project2 {

   public class Kepler {
      private static uint N = 0;

      public static void Run( KeplerProjectile p , bool Orbital ) {
         Console.WriteLine( "-=-=-=-=-=-" + "Points(V=" + Math.Round( p.Velocity , 2 ) + ").txt" + "-=-=-=-=-=-" );
         string path = @"Points(V="+Math.Round(p.Velocity,2)+").txt";

         StreamWriter file;
         bool writeable = true;
         if ( File.Exists( path ) && !Orbital ) {
            try {
               File.Delete( path );
            } catch ( IOException ) {
               Console.WriteLine( "Could not write file: " + path );
               writeable = false;
            }
         }
         if ( writeable ) {
            Console.WriteLine( "V_orbit = " + p.OrbitVelocity );
            Console.WriteLine( "V_escape = " + p.EscapeVelocity );
            Console.WriteLine( "Start = Pt[{0:0.000}, {1:0.000}], Velocity[{2:0.000},{3:0.000}]" , p.X , p.Y , p.VelocityX , p.VelocityY );
            Console.WriteLine( "Time(s): {0:0.000}, Range(m): {1:0.000}, Distance(m):{2:0.000}\nVelocity(m/s):{3:0.000}, Gravity(m/s^2):{4:0.000}\n" , p.Time , p.R , p.Distance , p.Velocity , p.Gravity );
            if ( p.Velocity > p.EscapeVelocity ) {
               Console.WriteLine( "Projectile too fast\nV:{0:0.000} EV:{1:0.00}" , p.Velocity , p.EscapeVelocity );
               Console.ReadKey();
               return;
            }
            double limit = 2*(1-p.DeltaStep)*(Math.Pow(p.Velocity, 0.333 ));

            if ( !Orbital ) {
               file = File.CreateText( path );
               file.WriteLine( "{0:0.000} {1:0.000}" , p.X , p.Y );
               while ( !p.Landed ) {
                  for ( int i = 0; i < limit && !p.Landed; i++ ) {
                     p.Step();
                  }
                  file.WriteLine( "{0:0.000} {1:0.000}" , p.X , p.Y );
               }
               file.Close();
            } else {
               while ( !p.Landed ) {
                  for ( int i = 0; i < 16 * limit && !p.Landed; i++ ) {
                     p.Step();
                  }
                  Console.WriteLine( "{0:0.000} {1:0.000} RADS: {2:0.000}" , p.X , p.Y , p.R );
               }
            }
            Console.WriteLine( "Finish = Pt[{0:0.000}, {1:0.000}], Velocity[{2:0.000},{3:0.000}]" , p.X , p.Y , p.VelocityX , p.VelocityY );
            Console.WriteLine( "Time(s): {0:0.000}, Range(m): {1:0.000}, Distance(m):{2:0.000}\nVelocity(m/s):{3:0.000}, Gravity(m/s^2):{4:0.000}\n" , p.Time , p.R , p.Distance , p.Velocity , p.Gravity );
         }
         Console.WriteLine( "-=-=-=-=-=-" + "End" + "-=-=-=-=-=-\n" );
      }

      public static void Orbital() {
         KeplerProjectile p1 = new KeplerProjectile(0,6.0E+6+5,1,10,0.5);
         KeplerProjectile p2 = new KeplerProjectile(0,6.0E+6+5, p1.OrbitVelocity,0,0.0001);
         Run( p2 , true );
      }

      public static void Main( string[] args ) {
         //Orbital();
         RvsTheta( 50 );
         RvsTheta( 150 );
         RvsTheta( 300 );
         RvsTheta( 1000 );
         RvsTheta( 2000 );
         RvsTheta( 3000 );
         Console.ReadKey();
      }

      public static void BasicCalculations() {
         KeplerProjectile p1 = new KeplerProjectile(0, 6.0E+6, 192, 45, 0.1); //First Projectile Fig 1
         KeplerProjectile p2 = new KeplerProjectile(0, 6.0E+6, 1000, 45, 0.2); //Second Projectile Fig 2
         KeplerProjectile p3 = new KeplerProjectile(0, 6.0E+6, 4000, 45, 0.3); //Third Projectile Fig 3
         KeplerProjectile p4 = new KeplerProjectile(0, 6.0E+6, 8000, 45, 0.4); //Fourth Projectile Fig 4
         KeplerProjectile p5 = new KeplerProjectile(0, 6.0E+6, 8600, 10, 0.5);
         KeplerProjectile p6 = new KeplerProjectile(0, 6.0E+6, 10000, 20, 0.5);
         KeplerProjectile p7 = new KeplerProjectile(0, 6.0E+6, 10600, 20, 0.5);
         KeplerProjectile p8 = new KeplerProjectile(0, 6.0E+6, 10500, 10, 0.5);
         KeplerProjectile p9 = new KeplerProjectile(0, 6.0E+6, 10700, 20, 0.5);
         KeplerProjectile p10 = new KeplerProjectile(0, 6.0E+6, 10800, 10, 0.5);
         KeplerProjectile p11 = new KeplerProjectile(0, 6.0E+6, 10900, 20, 0.5);
         KeplerProjectile p12 = new KeplerProjectile(0, 6.0E+6, 10910, 10, 0.5);
         Run( p1 , false );
         Run( p2 , false );
         Run( p3 , false );
         Run( p4 , false );
         Run( p5 , false );
         Run( p6 , false );
         Run( p7 , false );
         Run( p8 , false );
         Run( p9 , false );
         Run( p10 , false );
         Run( p11 , false );
         Run( p12 , false );
      }

      public static void RvsTheta( int velocity ) {
         String path = @"RvsTheta@V"+velocity+"A.txt";
         String path2 = @"RvsTheta@V"+velocity+"B.txt";
         StreamWriter file;
         StreamWriter file2;
         if ( File.Exists( path ) && File.Exists( path2 ) ) {
            try {
               File.Delete( path );
               File.Delete( path2 );
            } catch ( IOException ) {
               Console.WriteLine( "Could not write file(s): " + path + ", " + path2 );
            }
         }
         file = File.CreateText( path );
         file2 = File.CreateText( path2 );
         for ( int i = 0; i <= 89; i++ ) {
            KeplerProjectile p = new KeplerProjectile(0,6.0E+6, velocity, i,0.001);
            while ( !p.Landed ) {
               p.Step();
            }
            file.WriteLine( "{0:0.000} {1:0.000}" , i , p.R );
            file2.WriteLine( "{0:0.000} {1:0.000}" , i , p.R_Test );
         }
         file.Close();
         file2.Close();
      }

      public class KeplerProjectile {
         private const double G = 6.67E-11; //Gravitation Constant
         private const double Mass = 5.40E+24; //Mass of Oerth
         private const double Radius = 6.0E+6; //Radius of Oerth
         public double Xo { get; private set; } // Original X coordinate of the projectile
         public double Yo { get; private set; } // Original Y coordinate of the projectile
         public double X { get; private set; } // Current X coordinate of the projectile
         public double Y { get; private set; } // Current Y coordinate of the projectile
         public double Angle { get; private set; } // The angle given for the current projectile
         public double Magnitude { get { return Math.Sqrt( Math.Pow( X , 2 ) + Math.Pow( Y , 2 ) ); } } // The current magnitude (Distance) from center of Oerth
         public double MagnitudeO { get { return Math.Sqrt( Math.Pow( Xo , 2 ) + Math.Pow( Yo , 2 ) ); } } // The original magnitude from center of Oerth
         public double DeltaStep { get; private set; } // The change step between Coordinates of X and Y
         public double VelocityX { get; private set; } // Current X component of the velocity vector
         public double VelocityY { get; private set; } // Current Y component of the velocity vector
         public double Distance { get; private set; } // Current Total curvature distance traveled
         public double Time { get; private set; } // Current time traveled
         public double EscapeVelocity { get { return Math.Sqrt( ( 2 * G * Mass ) / Radius ); } } //Absolute maximum velocity or the projectile escapes orbit.
         public double OrbitVelocity { get { return Math.Sqrt( G * Mass / Magnitude ); } } //The velocity required for a successful orbit
         public double Velocity { get { return Math.Sqrt( Math.Pow( VelocityX , 2 ) + Math.Pow( VelocityY , 2 ) ); } } // Current velocity magnitude
         public double Gravity { get { return ( G * Mass ) / Math.Pow( Magnitude , 2 ); } } // The current gravity based on distance from Oerth
         public bool Landed { get { return ( Magnitude ) < Radius; } } // Returns the whether or not the projectile is below the radius, thus being landed.
         public double R { get { return Rads * Radius; } } // The actual R distance
         public double R_Test { get { return ( Math.Pow( Velocity , 2 ) * Math.Sin( 2 * ( ( Math.PI / 180 ) * Angle ) ) ) / Gravity; } } // The tested R distance using equation (1)

         public KeplerProjectile( double x , double y , double vel , double angle ) {
            this.Xo = ( this.X = x );
            this.Yo = ( this.Y = y );
            this.Angle = angle;
            this.VelocityX = vel * Math.Cos( ( Math.PI / 180 ) * angle );
            this.VelocityY = vel * Math.Sin( ( Math.PI / 180 ) * angle ); ;
            this.DeltaStep = 0.5;
         }

         public KeplerProjectile( double x , double y , double vel , double angle , double step ) : this( x , y , vel , angle ) {
            this.DeltaStep = step;
         }

         public double Rads {
            get {
               double COS_THETA = Math.Abs(( Xo * X + Yo * Y ) / ( Magnitude * MagnitudeO ));
               double RADS=0;
               double Xs = X != 0 ? X/Math.Abs(X) : 1;
               double Ys = Y != 0 ? Y/Math.Abs(Y) : 1;

               if ( Xs > 0 ) {
                  if ( Ys > 0 )
                     RADS = Math.Acos( COS_THETA );
                  else {
                     RADS = Math.PI - Math.Acos( COS_THETA );
                  }
               } else {
                  if ( Ys < 0 ) {
                     RADS = ( Math.PI ) + Math.Acos( COS_THETA );
                  } else {
                     RADS = 2 * ( Math.PI ) - Math.Acos( COS_THETA );
                  }
               }
               return RADS;
            }
         }

         private double AccelX() {
            return -( G * Mass * X ) / Math.Pow( Math.Pow( X , 2 ) + Math.Pow( Y , 2 ) , 3.0 / 2.0 );
         }

         private double AccelY() {
            return -( G * Mass * Y ) / Math.Pow( Math.Pow( X , 2 ) + Math.Pow( Y , 2 ) , 3.0 / 2.0 );
         }

         private double VelX() {
            return ( VelocityX = VelocityX + ( AccelX() * DeltaStep ) );
         }

         private double VelY() {
            return ( VelocityY = VelocityY + ( AccelY() * DeltaStep ) );
         }

         public bool Step() {
            this.Time += this.DeltaStep;
            this.Distance += Math.Sqrt( Math.Pow( X - StepX() , 2 ) + Math.Pow( Y - StepY() , 2 ) );
            return Landed;
         }

         private double StepX() {
            return ( X = X + ( VelX() * DeltaStep ) );
         }

         private double StepY() {
            return ( Y = Y + ( VelY() * DeltaStep ) );
         }
      }
   }
}