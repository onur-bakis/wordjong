using System.Collections;
using System.Collections.Generic;
using Scripts.Context.Signals;
using Scripts.Keys;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Managers
{
    public class WinManager : View
    {
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }
        public ParticleSystem[] particleSystem;

        private bool _isHighScore;
        protected override void OnEnable()
        {
            base.OnEnable();
            CoreGameSignals.onLevelFinished += InvokeParticles;
        }

        public void InvokeParticles(LevelFinishParams levelFinishParams)
        {
            _isHighScore = levelFinishParams.highScore;
            Invoke(nameof(PlayParticles),1f);
        }

        private void PlayParticles()
        {
            particleSystem[0].Play();
            particleSystem[1].Play();

            if (_isHighScore)
            {
                particleSystem[2].Play();
                particleSystem[3].Play();
            }
            else
            {
                particleSystem[4].Play();
            }
        }


    }
}