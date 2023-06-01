using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Diagnostics;

namespace ProjetTest.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            // Ajouter un gestionnaire d'�v�nements pour l'�v�nement "onClientResourceStart"
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            // V�rifier si la ressource en cours d'ex�cution est la m�me que la ressource pour laquelle nous avons enregistr� l'�v�nement
            if (GetCurrentResourceName() != resourceName) return;

            // Enregistrer la commande "/car"
            RegisterCommand("car", new Action<int, List<object>, string>((source, args, raw) =>
            {
                // V�rifier si un nom de voiture a �t� sp�cifi�
                if (args.Count > 0)
                {
                    string carName = args[0].ToString();
                    SpawnCar(carName);
                }
                else
                {
                    // Aucun nom de voiture n'a �t� sp�cifi�
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[CarSpawner]", "Utilisation : /car [nom de voiture]" }
                    });
                }
            }), false);
        }

        private void SpawnCar(string carName)
        {
            // V�rifier si la voiture existe dans la liste des v�hicules disponibles
            uint carHash = (uint)GetHashKey(carName);
            if (IsModelInCdimage(carHash))
            {
                // Charger le mod�le de la voiture
                RequestModel(carHash);

                // Attendre que le mod�le soit charg�
                while (!HasModelLoaded(carHash))
                {
                    Wait(100);
                }

                // Reste du code pour cr�er et placer le v�hicule
                // ...

                // Exemple de Debug.WriteLine pour un d�bogage
                System.Diagnostics.Debug.WriteLine($"Voiture {carName} apparue !");
            }
            else
            {
                // La voiture sp�cifi�e n'existe pas
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[CarSpawner]", "La voiture sp�cifi�e n'existe pas." }
                });
            }
        }
    }
}