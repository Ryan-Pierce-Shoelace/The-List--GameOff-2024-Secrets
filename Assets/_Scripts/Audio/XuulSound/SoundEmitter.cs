using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEditor;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Shoelace.Audio.XuulSound
{
	public class SoundEmitter : MonoBehaviour
	{
		[Header("Sound Configuration")]
		[SerializeField] private SoundConfig soundConfig;

		[Header("Behavior")]
		[SerializeField] private EmitterGameEvent playTrigger = EmitterGameEvent.None;

		[SerializeField] private EmitterGameEvent stopTrigger = EmitterGameEvent.None;

		[SerializeField] private bool playOnStart;
		[SerializeField] private bool allowFadeout = true;
		[SerializeField] private bool triggerOnce;
		[SerializeField] private bool preload;

		[Header("3D Settings")]
		[SerializeField] private bool overrideAttenuation;

		[SerializeField] private float overrideMinDistance = -1f;
		[SerializeField] private float overrideMaxDistance = -1f;

		[Header("Debug Visualization")]
		[SerializeField] private bool showGizmos = true;

		[SerializeField] private Color gizmoColor = new Color(1f, 0.5f, 0f, 0.75f);
		private bool hasTriedLoadingInEditor;

		private Dictionary<string, float> currentParameters = new();

		private EventDescription eventDescription;
		private EventInstance instance;
		private bool hasTriggered;
		private bool isOneshot;
		private float minDistance;
		private float maxDistance;
		private bool hasLoadedDistances;

		protected virtual void Start()
		{
			InitializeEvent();

			if (playOnStart)
			{
				Play();
			}
		}

		private void InitializeEvent()
		{
			if (soundConfig == null || soundConfig.EventRef.IsNull) return;

			eventDescription = RuntimeManager.GetEventDescription(soundConfig.EventRef);

			if (!eventDescription.isValid()) return;

			eventDescription.isOneshot(out isOneshot);
			LoadDistances();
			InitializeParameters();
		}


		private void LoadDistances()
		{
			if (!eventDescription.isValid()) return;

			eventDescription.getMinMaxDistance(out minDistance, out maxDistance);

			if (overrideAttenuation)
			{
				minDistance = overrideMinDistance;
				maxDistance = overrideMaxDistance;
			}

			hasLoadedDistances = true;
		}

		private void InitializeParameters()
		{
			currentParameters.Clear();
			foreach (SoundConfig.ParameterConfig param in soundConfig.Parameters)
			{
				currentParameters[param.Name] = param.DefaultValue;
			}
		}


		#region Basic Controls

		public void Play()
		{
			if (triggerOnce && hasTriggered) return;
			if (soundConfig == null || soundConfig.EventRef.IsNull) return;

			if (!eventDescription.isValid())
			{
				eventDescription = RuntimeManager.GetEventDescription(soundConfig.EventRef);
				LoadDistances();
			}

			CreateInstance();
			ConfigureInstance();
			instance.start();

			hasTriggered = true;
		}

		public void Stop()
		{
			if (!instance.isValid()) return;

			instance.stop(allowFadeout ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE);
			instance.release();

			if (!allowFadeout)
			{
				instance.clearHandle();
			}
		}

		public void UpdateVolume(float newVolume)
		{
			if (!instance.isValid()) return;
			instance.setVolume(newVolume);
		}

		#endregion


		private void CreateInstance()
		{
			if (!eventDescription.isValid()) return;

			if (isOneshot && instance.isValid())
			{
				instance.release();
				instance.clearHandle();
			}

			eventDescription.createInstance(out instance);
		}


		private void ConfigureInstance()
		{
			if (!instance.isValid()) return;

			Configure3DAttributes();
			ConfigureParameters();
			instance.setVolume(soundConfig.DefaultVolume);
		}

		private void Configure3DAttributes()
		{
			if (!soundConfig.Is3D) return;

			var rigidBody2D = GetComponent<Rigidbody2D>();
			if (rigidBody2D != null)
			{
				instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, rigidBody2D));
				RuntimeManager.AttachInstanceToGameObject(instance, gameObject, rigidBody2D);
			}
			else
			{
				instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
				RuntimeManager.AttachInstanceToGameObject(instance, gameObject);
			}

			if (overrideAttenuation)
			{
				instance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, overrideMinDistance);
				instance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, overrideMaxDistance);
			}
		}


		protected virtual void HandleGameEvent(EmitterGameEvent gameEvent)
		{
			if (playTrigger == gameEvent)
			{
				Play();
			}

			if (stopTrigger == gameEvent)
			{
				Stop();
			}
		}

		#region Params

		private void ConfigureParameters()
		{
			foreach (KeyValuePair<string, float> param in currentParameters)
			{
				instance.setParameterByName(param.Key, param.Value);
			}
		}

		public void SetParameter(string paramName, float value)
		{
			if (!currentParameters.ContainsKey(paramName))
			{
				Debug.LogWarning($"Parameter {paramName} not found in sound config");
				return;
			}

			currentParameters[paramName] = value;

			if (instance.isValid())
			{
				instance.setParameterByName(paramName, value);
			}
		}

		public float GetParameter(string paramName)
		{
			return currentParameters.GetValueOrDefault(paramName, 0f);
		}

		#endregion

		private void OnValidate()
		{
			if (!overrideAttenuation) return;
			overrideMinDistance = Mathf.Max(-1f, overrideMinDistance);
			overrideMaxDistance = Mathf.Max(overrideMinDistance, overrideMaxDistance);
		}

		public bool IsPlaying()
		{
			if (!instance.isValid()) return false;

			instance.getPlaybackState(out PLAYBACK_STATE state);
			return state != PLAYBACK_STATE.STOPPED;
		}

		private void OnDisable()
		{
			if (instance.isValid())
			{
				Stop();
			}
		}

		private void OnDestroy()
		{
			if (instance.isValid())
			{
				instance.stop(STOP_MODE.IMMEDIATE);
				instance.release();
			}

			if (preload && eventDescription.isValid())
			{
				eventDescription.unloadSampleData();
			}
		}

		#region Gizmos

		private void OnDrawGizmos()
		{
			DrawGizmosInternal(false);
		}

		private void OnDrawGizmosSelected()
		{
			DrawGizmosInternal(true);
		}

		private void DrawGizmosInternal(bool selected)
		{
			if (!showGizmos || !soundConfig || !soundConfig.Is3D) return;

			// Try to load event description in editor
			if (!Application.isPlaying && !hasLoadedDistances && !hasTriedLoadingInEditor)
			{
				hasTriedLoadingInEditor = true;
				eventDescription = RuntimeManager.GetEventDescription(soundConfig.EventRef);
				if (eventDescription.isValid())
				{
					LoadDistances();
				}
#if UNITY_EDITOR
				else
				{
					// Force FMOD to update in editor if needed
					RuntimeManager.StudioSystem.flushCommands();
					eventDescription = RuntimeManager.GetEventDescription(soundConfig.EventRef);
					if (eventDescription.isValid())
					{
						LoadDistances();
					}
				}
#endif
			}

			// Set color based on selection state
			Color currentColor = selected ? gizmoColor : new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, gizmoColor.a * 0.6f);
			Gizmos.color = currentColor;

			DrawEmitterGizmo();
		}

		private void DrawEmitterGizmo()
		{
			if (!hasLoadedDistances) return;

			// Draw min distance sphere
			if (minDistance > 0)
			{
				// Inner filled sphere
				Color fillColor = Gizmos.color;
				fillColor.a *= 0.1f;
				Gizmos.color = fillColor;
				Gizmos.DrawSphere(transform.position, minDistance);

				// Outer wire sphere
				Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, gizmoColor.a);
				Gizmos.DrawWireSphere(transform.position, minDistance);
			}

			// Draw max distance sphere
			if (maxDistance > 0)
			{
				DrawDottedSphere(maxDistance);
			}

			// Draw direction indicator
			DrawDirectionIndicator();
		}

		private void DrawDottedSphere(float radius)
		{
			const int segments = 32;
			const float dashSize = 0.2f;

			for (int i = 0; i < segments; i++)
			{
				float angle = i * Mathf.PI * 2 / segments;
				float nextAngle = (i + 1) * Mathf.PI * 2 / segments;

				// Only draw every other segment for dotted effect
				if (i % 2 == 0)
				{
					// Draw circles in all three planes
					DrawDottedArc(angle, nextAngle, radius, Vector3.up);
					DrawDottedArc(angle, nextAngle, radius, Vector3.right);
					DrawDottedArc(angle, nextAngle, radius, Vector3.forward);
				}
			}
		}

		private void DrawDottedArc(float startAngle, float endAngle, float radius, Vector3 axis)
		{
			Vector3 center = transform.position;
			Vector3 start = center + (Quaternion.AngleAxis(startAngle * Mathf.Rad2Deg, axis) * Vector3.Cross(axis, Vector3.up)) * radius;
			Vector3 end = center + (Quaternion.AngleAxis(endAngle * Mathf.Rad2Deg, axis) * Vector3.Cross(axis, Vector3.up)) * radius;

			Gizmos.DrawLine(start, end);
		}

		private void DrawDirectionIndicator()
		{
			float arrowLength = Mathf.Min(minDistance > 0 ? minDistance * 0.25f : maxDistance * 0.1f, 1f);
			Vector3 direction = transform.forward * arrowLength;
			Vector3 right = transform.right * (arrowLength * 0.3f);

			Gizmos.DrawRay(transform.position, direction);
			Gizmos.DrawRay(transform.position + direction, -direction * 0.3f + right);
			Gizmos.DrawRay(transform.position + direction, -direction * 0.3f - right);
		}

		#endregion
	}
}