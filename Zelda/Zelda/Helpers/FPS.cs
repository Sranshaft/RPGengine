﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Zelda.Helpers
{
    class FPS
    {
        public float Value { get; private set; }

        private TimeSpan _sample;
        private Stopwatch _stopWatch;
        private int _frames;

        public FPS()
        {
            this.Value = 0;

            _sample = TimeSpan.FromSeconds(1);
            _frames = 0;
            _stopWatch = Stopwatch.StartNew();
        }

        public void Update()
        {
            if (_stopWatch.Elapsed > _sample)
            {
                Global.DEBUG_FPS = this.Value;

                this._stopWatch.Reset();
                this._stopWatch.Start();

                _frames = 0;
            }
            else
            {
                _frames++;
            }

            this.Value = (float)(_frames / _stopWatch.Elapsed.TotalSeconds);
        }

        public bool IsRunning()
        {
            return _stopWatch.IsRunning;
        }

        public void StartMonitor()
        {
            _stopWatch.Start();
        }

        public void StopMonitor()
        {
            _stopWatch.Stop();
            _stopWatch.Reset();
        }
    }
}
