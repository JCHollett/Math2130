using System;

namespace Math2130Project4 {

   public class Pair<K, V> {
      public K k { get; set; }
      public V v { get; set; }

      public Pair( K k , V v ) {
         this.k = k;
         this.v = v;
      }
   }

   public class Car {
      private static uint ID = 1;
      private uint vmax;
      public static uint R = 0;
      public uint V { get; set; }
      public Road Location { get; private set; }
      public uint VMax { get { return this.vmax; } set { this.vmax = value; } }
      public uint UID { get; private set; }
      public Pair<Road , uint> Next { get; private set; }

      public Car( Road cur ) {
         this.Location = cur;
         this.UID = ID++;
         this.vmax = Track.SpeedLimit;
      }

      public bool Calculate() {
         this.Next = this.GetMove();
         if ( VMax == 0 ) {
            this.Next.k++;
         }
         return this.Next != null;
      }

      public bool Move() {
         if ( this.Next != null ) {
            this.Location.Car = null;
            this.Location = this.Next.k;
            this.Next.k.Car = this;
            this.Next = null;
            return true;
         }
         return false;
      }

      private Pair<Road , uint> GetMove() {
         if ( this.V < this.VMax ) {
            this.V = V + 1;
         } else {
            this.V = this.VMax;
         }
         Pair<Road,uint> result = null;
         if ( ( result = Check( this.V ) ).k.Car != null ) {
            if ( V > 0 ) {
               this.V = result.v - 1;
            }
            result.k--;
         }
         if ( V > 0 ) {
            if ( Decelerate( Rand() ) ) {
               this.V--;
               result.k--;
            }
         }
         return result;
      }

      private Pair<Road , uint> Check( uint V ) {
         Road road = this.Location;
         uint i = 0;
         V++;
         while ( --V > 0 ) {
            if ( road.Next.Car != null ) {
               return new Pair<Road , uint>( ++road , ++i );
            } else {
               ++road; ++i;
            }
         }
         return new Pair<Road , uint>( road , i );
      }

      private double Rand() {
         Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
         return rndNum.NextDouble();
      }

      public bool Decelerate( double P ) {
         if ( P < ( this.Location.Track.P_o + this.Location.P_o ) ) {
            return true;
         } else {
            return false;
         }
      }

      public override string ToString() {
         return "(" + this.Location.Index + ")";
      }
   }
}