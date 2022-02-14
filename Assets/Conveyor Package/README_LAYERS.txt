**Physics Layers**

Conveyor, Main Box, ScoreChecker, Toy

GameObject Name		|  Layer Name
------------------------------------------
Conveyor			|  [Conveyor]
Main Box 			|  [Box]
Main Box Colliders  |  [Box]
ScoreChecker 		|  [ScoreChecker]
Robot Prefab 		|  [Toy]
Robot Colliders 	|  [Grabbable]


-------------------------------------------------------------------------------------------------------------------------------------------


**Layer Matrix**

			|Default   Conveyor		ScoreChecker	Toy		Box		Grabbable
---------------------------------------------------------------------------------------
Default		|   X		  O				 O			 O		 O		    O
Conveyor	|   O		  X				 X			 X		 O		    O
ScoreChecker|   O		  X				 X			 O		 X		    X 
Toy			|   O		  X				 O			 O		 O		    O
Box			|   O		  O				 X			 O		 O		    O
Grabbable	|   O		  O				 X			 O		 O		    O