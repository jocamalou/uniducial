/*
Copyright (c) 2010 Andr� Gr�schel

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUIO;
namespace UniducialLibrary
{
    public class TuioManager : TuioListener
    {
        private static TuioManager instance;
        private bool isConnected;

        public static TuioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    new TuioManager();
                }

                return instance;
            }
        }


        private TuioClient client;
        private Dictionary<int, TuioObject> tuioObjects;

        //default port
        private int tuioPort = 3333;

        public TuioManager()
        {
            if (instance != null)
            {
                Debug.LogError("Trying to create two instances of singleton.");
                return;
            }

            instance = this;
            isConnected = false;

            //init members
            tuioObjects = new Dictionary<int, TuioObject>();

            //setup TUIO client connection
            client = new TuioClient(tuioPort);
            client.addTuioListener(this);
        }

        ~TuioManager()
        {
            disconnect();
        } 

        #region TUIOListener methods
        void TuioListener.addTuioObject(TuioObject tobj)
        {
            tuioObjects.Add(tobj.getSymbolID(), tobj);
        }

        void TuioListener.updateTuioObject(TuioObject tobj)
        {
        }

        void TuioListener.removeTuioObject(TuioObject tobj)
        {
            tuioObjects.Remove(tobj.getSymbolID());
        }

        void TuioListener.addTuioCursor(TuioCursor tcur)
        {
            // throw new System.NotImplementedException();
        }

        void TuioListener.updateTuioCursor(TuioCursor tcur)
        {
            //throw new System.NotImplementedException();
        }

        void TuioListener.removeTuioCursor(TuioCursor tcur)
        {
            //throw new System.NotImplementedException();
        }

        void TuioListener.refresh(TuioTime ftime)
        {
            //throw new System.NotImplementedException();
        }
        #endregion

        public void connect()
        {
            client.connect();

            if (client.isConnected())
            {
                Debug.Log("TUIO connection on port " + tuioPort + " established.");
                isConnected = true;
            }
            else
            {
                Debug.LogError("Could not establish a TUIO connection on port " + tuioPort + ".");
            }
        }

        //TODO: remove method
        public List<TuioObject> mGetClients()
        {
            return client.getTuioObjects();
        }

        public bool isMarkerAlive(int markerID)
        {
            return tuioObjects.ContainsKey(markerID);
        }


        public TuioObject getMarker(int markerID)
        {
            if (tuioObjects.ContainsKey(markerID))
            {
                return tuioObjects[markerID];
            }
            else
            {
                return null;
            }
        }

        public int getObjectCount()
        {
            return tuioObjects.Count;
        }

        public void disconnect()
        {
            client.removeTuioListener(this);
            client.disconnect();
            Debug.Log("TUIO connection on port " + tuioPort + "  closed.");
            isConnected = false;
        }

        public bool IsConnected
        {
            get { return isConnected; }
        }

        public int TuioPort
        {
            get { return tuioPort; }
            set { tuioPort = value; }
        }
    }
}