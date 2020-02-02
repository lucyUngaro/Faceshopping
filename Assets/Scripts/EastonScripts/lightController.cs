﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightController : MonoBehaviour
{

    //lerps everything in its list to the color of the sun
    public Color sunrise, midday, sunset, midnight, currentColor;
    public List<SpriteRenderer> lightSprites = new List<SpriteRenderer>();
    public List<float> spriteAlphas = new List<float>();
    public List<ParticleSystem> lightParts = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < lightSprites.Count; i++){
            spriteAlphas.Add(lightSprites[i].color.a);
        }

    }

    public void AddSprite(SpriteRenderer sprite)
    {
        lightSprites.Add(sprite);
        spriteAlphas.Add(sprite.color.a);
    }

    public void RemoveSprite(SpriteRenderer sprite)
    {
        lightSprites.Remove(sprite);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lightSprites.Count; i++){

            if (lightSprites[i] != null)
            {
                lightSprites[i].color = Color.Lerp(lightSprites[i].color, currentColor, Time.deltaTime);
                lightSprites[i].color = new Color(lightSprites[i].color.r, lightSprites[i].color.g, lightSprites[i].color.b, spriteAlphas[i]);
            }
        }

        for (int i = 0; i < lightParts.Count; i++)
        {
            var main = lightParts[i].main;
            Color partColor = Color.Lerp(main.startColor.colorMin, currentColor, Time.deltaTime);
            main.startColor = new ParticleSystem.MinMaxGradient(partColor, partColor);
        }

        //0-45 sunrise to midday
        //45- 90 midday to sunset
        //90 - 135 sunset to midnight
        //135 - 225 midnight to sunrise
        if(TimeControl.time/45 <= 1){
            currentColor = Color.Lerp(sunrise, midday, TimeControl.time / 45);
            TimeAmbience.timeAmbience.playDaySound();
        }else if(TimeControl.time/90 < 1){
            currentColor = Color.Lerp(midday, sunset, TimeControl.time/45 - 1);
            TimeAmbience.timeAmbience.playDaySound();
        }
        else if (TimeControl.time/135 < 1)
        {
            currentColor = Color.Lerp(sunset, midnight, TimeControl.time/45 - 2);
            TimeAmbience.timeAmbience.playNightSound();
        }
        else if (TimeControl.time/225 < 1)
        {
            currentColor = Color.Lerp(midnight, sunrise, TimeControl.time/45 - 3);
            TimeAmbience.timeAmbience.playNightSound();
        }

    }
}
