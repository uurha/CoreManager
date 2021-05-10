﻿#region license

// Copyright 2021 Arcueid Elizabeth D'athemon
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//     http://www.apache.org/licenses/LICENSE-2.0 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.CustomAttributes.Editor
{
    [InitializeOnLoad]
    internal static class BuildState
    {
        static BuildState()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(CheckSceneObjects);
        }

        private static void CheckSceneObjects(BuildPlayerOptions buildPlayerOptions)
        { 
            var errorObjectPairs = Enumerable.Empty<ErrorObjectPair>();
            errorObjectPairs = errorObjectPairs.Concat(CheckSceneObjects());
            errorObjectPairs = errorObjectPairs.Concat(CheckPrefabs());
            
            var errors = errorObjectPairs as ErrorObjectPair[] ?? errorObjectPairs.ToArray();

            if (!errors.Any())
            {
                BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(buildPlayerOptions);
                return;
            }
            EditorApplication.Beep();
            foreach (var error in errors) Debug.LogException(new BuildPlayerWindow.BuildMethodException(error.Key));
        }

        private static IEnumerable<ErrorObjectPair> CheckPrefabs()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();
            var objs =
                allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject)
                         .Zip(allAssets, (o, s) => new {obj = o, path = s} );

            var errors = Enumerable.Empty<ErrorObjectPair>();

            return objs.Where(x => x.obj != null)
                       .Aggregate(errors, (current, value) => 
                                              current.Concat(Validation.ErrorObjectPairs(value.obj)
                                                                                       .Select(x =>
                                                                                               {
                                                                                                   x.Key += $"\n<b>Prefab path:</b> <i>\"{value.path}\"</i>";
                                                                                                   return x;
                                                                                               })));
        }

        private static IEnumerable<ErrorObjectPair> CheckSceneObjects()
        {
            var errors = Enumerable.Empty<ErrorObjectPair>();
            var openScene = EditorSceneManager.GetActiveScene().path;

            if (EditorBuildSettings.scenes.Length > 0)
                foreach (var s in EditorBuildSettings.scenes)
                {
                    if (!s.enabled) continue;
                    var scene = EditorSceneManager.OpenScene(s.path);

                    errors = scene.GetRootGameObjects()
                                  .Aggregate(errors, (current, gameObject) =>
                                                         current.Concat(Validation.ErrorObjectPairs(gameObject)
                                                                                  .Select(x =>
                                                                                          {
                                                                                              x.Key += $"\n<b>Scene path</b>: <i>\"{s.path}\"</i>";
                                                                                              return x;
                                                                                          })));
                }
            else
                errors = SceneManager.GetActiveScene().GetRootGameObjects()
                                     .Aggregate(errors, (current, rootGameObject) => current.Concat(Validation.ErrorObjectPairs(rootGameObject)));
            EditorSceneManager.OpenScene(openScene);
            return errors;
        }
    }
}
