namespace Math2130Project4 {

   using System;

   public class Road {
      public static uint I = 0;
      public double P_o { get; private set; }
      public uint Index { get; private set; }
      public Road Previous { get; internal set; }
      public Road Next { get; internal set; }
      public uint Length { get; private set; }
      public Car Car { get; internal set; }
      public Track Track { get; private set; }

      public static double ND( uint Index ) {
         double k = 20;
         double d = 65;
         double u = 350;
         return k * Math.Pow( Math.E , ( ( -1.0 / 2.0 ) * Math.Pow( ( Index - u ) / d , 2.0 ) ) ) / ( d * Math.Sqrt( 2.0 * Math.PI ) );
      }

      public Road( Road p , Road n , bool car , bool ND ) : this( p , n , ND ) {
         this.Car = car ? new Car( this ) : null;
      }

      public Road( Road p , Road n , bool ND ) : this( ND ) {
         if ( p != null ) {
            this.Previous = p;
            p.Next = this;
         }
         if ( n != null ) {
            this.Next = n;
            n.Previous = this;
         }
      }

      public Road( bool car , bool ND ) : this( ND ) {
         this.Car = car ? new Car( this ) : null;
      }

      public Road( bool ND ) {
         this.Index = I++;
         this.Previous = null;
         this.Next = null;
         this.Car = null;
         if ( ND ) {
            this.P_o = Road.ND( Index );
         } else {
            this.P_o = 0.0;
         }
      }

      public static Road operator ++( Road k ) {
         return k.Next;
      }

      public static Road operator --( Road k ) {
         return k.Previous;
      }

      internal Road SetTrack( Track track ) {
         this.Track = track;
         return this;
      }

      public override string ToString() {
         return "(" + this.Previous.Index + "," + ( this.Previous.Car != null ? "C" : "N" ) + " )->(" + this.Index + "," + ( this.Car != null ? "C" : "N" ) + ")->(" + this.Next.Index + "," + ( this.Next.Car != null ? "C" : "N" ) + ")";
      }

      public string ToSimpleString() {
         return ( this.Car != null ? this.Car.UID.ToString() : " " );
      }
   }
}