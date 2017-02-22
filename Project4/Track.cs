namespace Math2130Project4 {

   public class Track {
      public Road[] Roads;
      public Car[] Cars;
      public static int n = 0;
      public static uint SpeedLimit = 5;
      public double P_o { get; private set; }

      public Track( int Ncars , int Nroads , uint spdlmt , double p , bool ND ) {
         int CarSpacing = ((Nroads) / Ncars);
         this.Roads = new Road[ Nroads ];
         this.Cars = new Car[ Ncars ];
         this.ConnectRoads( CarSpacing , Ncars , ND );
         SpeedLimit = spdlmt;
         this.P_o = p;
         Road.I = 0;
      }

      private void ConnectRoads( int spacing , int Ncar , bool ND ) {
         for ( int i = 0; i < Roads.Length; i++ ) {
            bool car = i % spacing == 0 && Ncar-- > 0;
            if ( i == 0 ) {
               Roads[ i ] = new Road( car , ND ).SetTrack( this );
            } else if ( i < Roads.Length - 1 ) {
               Roads[ i ] = new Road( Roads[ i - 1 ] , null , ( car ) , ND ).SetTrack( this );
            } else {
               Roads[ i ] = new Road( Roads[ i - 1 ] , Roads[ 0 ] , ( car ) , ND ).SetTrack( this );
            }
            if ( car ) {
               this.Cars[ ( this.Cars.Length - Ncar ) - 1 ] = Roads[ i ].Car;
            }
         }
      }
   }
}