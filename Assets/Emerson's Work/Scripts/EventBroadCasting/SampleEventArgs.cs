using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class SampleEventArgs : EventArgs
{
    //add properties for tis eventArgs; to be passed in the sampleReceiver
    private int nSample;
    private bool bSample;

    //constructor; with default values
    public SampleEventArgs(int nSample = 0, bool bSample = true)
    {
        this.nSample = nSample;
        this.bSample = bSample;
    }

    //Property setting; "read-only-property"
    public int Nsample
    {
        get { return this.nSample; }
    }
    //Property setting; "read-only-property"
    public bool Bsample
    {
        get { return this.bSample; }
    }
}
