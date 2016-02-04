﻿var target : Transform;
 var gravity : float = 20;
 var moveSpeed : float = 6;  // chase speed
 var rotSpeed : float = 90;  // speed to turn to the player (degrees/second)
 var attackDistance : float = 2;  // attack distance
 var detectRange : float = 20;  // detection distance
 
 private var transf : Transform;
 private var character: CharacterController; 
 
 function Start () { 
     if (!target) target = GameObject.FindWithTag ("Player").transform; 
     transf = transform;
     character = GetComponent(CharacterController);
 }
 
 function Update(){
     if (target){
         var tgtDir = target.position - transf.position;
         var tgtDist = tgtDir.magnitude; // get distance to the target
         if (!Physics.Raycast(transf.position, tgtDir, detectRange)){
             // stays in idle mode if can't see target
             Idle(); 
         }
         else {
             var moveDir = target.position - transf.position;
             moveDir.y = 0;  // prevents enemy tilting
             rot = Quaternion.FromToRotation(Vector3.forward, moveDir);
             transf.rotation = Quaternion.RotateTowards(transf.rotation, rot, rotSpeed * Time.deltaTime);
             if (tgtDist <= attackDistance){  // if dist <= attackDistance: stop and attack
                 // do your attack here
                 print("Attack!");
             }
             else {  // if attackDistance < dist < detectRange: chase the player
                 // Move towards target
                 MoveCharacter(moveDir, moveSpeed);
             }
         }
     }
 }
 
 var walkSpeed = 3.0; 
 var travelTime = 2.0; 
 var idleTime = 1.5;
 var rndAngle = 45;  // enemy will turn +/- rndAngle
 
 private var timeToNewDir = 0.0;
 private var turningTime = 0.0;
 private var turn: float;
 
 function Idle () { 
     // Walk around and pause in random directions 
     if (Time.time > timeToNewDir) { // time to change direction?
         turn = (Random.value > 0.5)? rndAngle : -rndAngle; // choose new direction
         turningTime = Time.time + idleTime; // will stop and turn during idleTime...
         timeToNewDir = turningTime + travelTime;  // and travel during travelTime 
     }
     if (Time.time < turningTime){ // rotate during idleTime...
         transform.Rotate(0, turn*Time.deltaTime/idleTime, 0);
     } else {  // and travel until timeToNewDir
         MoveCharacter(transform.forward, walkSpeed);
     }
 }
 
 function MoveCharacter(dir: Vector3, speed: float){
 
     var vel = dir.normalized * speed;  // vel = velocity to move 
     // clamp the current vertical velocity for gravity purpose
     vel.x = Mathf.Clamp(character.velocity.y, -30, 2); 
     vel.x -= gravity * Time.deltaTime;  // apply gravity
     character.Move(vel * Time.deltaTime);  // move
 }