﻿using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotAcademy : Academy { 
    public enum RobotControl {
        player, python
    }

    public enum DataCollection {
        gate, path
    }

    [Header("Controller settings")]
    public RobotControl control;
    public Brain learningBrain;
    public Brain playerBrain;

    [Header("Data collection settings")]
    public DataCollection mode;
    public GameObject gateTargetObject;
    public GameObject pathTargetObject;

    [Header("Debug settings")]
    public bool forceDataCollection = false;
    public bool forceNoise = false;

    RobotAgent robot;

    void OnValidate() {
        robot = GameObject.Find("Robot").GetComponent<RobotAgent>();
        if (control == RobotControl.player) {
            robot.GiveBrain(playerBrain);
            broadcastHub.broadcastingBrains.Clear();
            broadcastHub.broadcastingBrains.Add(playerBrain);
            robot.playerSteering = true;
        }
        else {
            robot.GiveBrain(learningBrain);
            broadcastHub.broadcastingBrains.Clear();
            broadcastHub.broadcastingBrains.Add(learningBrain);
            broadcastHub.SetControlled(learningBrain, true);
            robot.playerSteering = false;
        }
        if (resetParameters["CollectData"] == 1 || forceDataCollection) {
            robot.sendRelativeData = true;
            robot.dataCollection = true;
            robot.mode = mode;
            robot.gateTargetObject = gateTargetObject;
            robot.pathTargetObject = pathTargetObject;
        }
        else {
            robot.sendRelativeData = false;
            robot.dataCollection = false;
        }
    }

    public override void InitializeAcademy() {
        if (control == RobotControl.player) {
            robot.agentParameters.maxStep = 0;
        }
        if (resetParameters["CollectData"] == 1 || forceDataCollection) {
            if (resetParameters["EnableNoise"] == 1 || forceNoise)
                robot.addNoise = true;
            else
                robot.addNoise = false;
        }
    }
}
