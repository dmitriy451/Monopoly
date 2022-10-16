﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Data to change")]
    public List<Transform> targets;
    public float duration;
    public float magnitude;
    public float constantMagnitude;

    [Header("Internal")]
    public bool isDoingConstantShake = false;
    public bool isShaking = false;
    public List<Vector3> origPositions;
    public float constantPercentAmplitude;

    private void Start()
    {
        if (TryGetComponent(out CameraController _cameraMover))
        {
            targets = new List<Transform>();
            targets.Add(_cameraMover.transform);
        }

        origPositions.Clear();
        for (int i = 0; i < targets.Count; i++)
            origPositions.Add(targets[i].localPosition);
    }

    public void Shake(float duration = 0f, float magnitude = 0f)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        if (isShaking)
            yield break;

        duration = duration <= 0f ? this.duration : duration;
        magnitude = magnitude <= 0f ? this.magnitude : duration;

        if (!isDoingConstantShake)
        {
            origPositions.Clear();
            for (int i = 0; i < targets.Count; i++)
                origPositions.Add(targets[i].localPosition);
        }

        isShaking = true;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            var rand = Random.insideUnitCircle;
            float x = rand.x * magnitude * ((duration - elapsed) / duration);
            float y = rand.y * magnitude * ((duration - elapsed) / duration);

            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].localPosition =
                    new Vector3(
                        origPositions[i].x + x,
                        origPositions[i].y + y,
                        origPositions[i].z);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < targets.Count; i++)
            targets[i].localPosition = origPositions[i];

        isShaking = false;
    }
}
