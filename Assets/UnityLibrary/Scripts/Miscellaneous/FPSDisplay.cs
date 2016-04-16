﻿using System;
using UnityEngine;

namespace Miscellaneous
{
    /// <summary>
    /// Displays an fps counter. In the editor, values might be lower than they should (since scene is rendered twice).
    /// </summary>
    public class FPSDisplay : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Color TextColor = new Color(1, 1, 1, 1);
        [SerializeField]
        private TextAnchor Anchor = TextAnchor.UpperLeft;
        [SerializeField]
        private float Size = 2f;
        [SerializeField]
        private int GuiDepth = -1000;

        private float deltaTime = 0.0f;

        // .. OPERATIONS

        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f; //moving average (90% history, 10% current)
        }

        void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            var size = (int)Math.Round(h * Size * 0.01f);

            Rect rect = new Rect(0, 0, w, size);
            style.alignment = Anchor;
            style.fontSize = size;
            style.normal.textColor = TextColor;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

            GUI.depth = GuiDepth;
            GUI.Label(rect, text, style);
        }
    }
}
