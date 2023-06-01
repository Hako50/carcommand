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
            // Ajouter un gestionnaire d'événements pour l'événement "onClientResourceStart"
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            // Vérifier si la ressource en cours d'exécution est la même que la ressource pour laquelle nous avons enregistré l'événement
            if (GetCurrentResourceName() != resourceName) return;

            // Enregistrer la commande "/car"
            RegisterCommand("car", new Action<int, List<object>, string>((source, args, raw) =>
            {
                // Vérifier si un nom de voiture a été spécifié
                if (args.Count > 0)
                {
                    string carName = args[0].ToString();
                    SpawnCar(carName);
                }
                else
                {
                    // Aucun nom de voiture n'a été spécifié
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
            // Vérifier si la voiture existe dans la liste des véhicules disponibles
            uint carHash = (uint)GetHashKey(carName);
            if (IsModelInCdimage(carHash))
            {
                // Charger le modèle de la voiture
                RequestModel(carHash);

                // Attendre que le modèle soit chargé
                while (!HasModelLoaded(carHash))
                {
                    Wait(100);
                }

                // Reste du code pour créer et placer le véhicule
                // ...

                // Exemple de Debug.WriteLine pour un débogage
                System.Diagnostics.Debug.WriteLine($"Voiture {carName} apparue !");
            }
            else
            {
                // La voiture spécifiée n'existe pas
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[CarSpawner]", "La voiture spécifiée n'existe pas." }
                });
            }
        }
    }
}