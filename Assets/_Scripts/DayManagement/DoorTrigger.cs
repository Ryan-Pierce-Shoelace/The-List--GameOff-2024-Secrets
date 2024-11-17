using System;
using System.Collections.Generic;
using System.Linq;
using Horror.Chores;
using Horror.RoomNavigation;
using Shoelace.Audio.XuulSound;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DayManagement
{
	public class DoorTrigger : MonoBehaviour
	{
		[SerializeField] private Doorway doorwayToTrigger;

		[SerializeField] private SoundConfig doorSlamSound;
		[SerializeField] private SoundConfig openDoorSound;
		[SerializeField] private SoundConfig closedDoorSound;

		[SerializeField] private List<DoorTriggerEvent> eventsToTrigger = new List<DoorTriggerEvent>();


		private void OnEnable()
		{
			ChoreEvents.OnChoreCompleted += TriggerDoor;
		}

		private void OnDisable()
		{
			ChoreEvents.OnChoreCompleted -= TriggerDoor;
		}

		private void TriggerDoor(string choreName)
		{
			DoorTriggerEvent toTrigger = eventsToTrigger.FirstOrDefault(e => e.ID == choreName);

			if (toTrigger is { WillTriggerImmediately: true })
			{
				TriggerDoor(toTrigger.DoorActionToDo);
			}
		}


		private void TriggerDoor(StateToSet state)
		{
			switch (state)
			{
				case StateToSet.Close:
					CloseDoorway();
					break;
				case StateToSet.Open:
					OpenDoorway();
					break;
				case StateToSet.Slam:
					SlamDoorway();
					break;
				default:
					OpenDoorway();
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}


		[Button]
		public void SlamDoorway()
		{
			if (doorwayToTrigger.IsDoorClosed) return;

			doorwayToTrigger?.SetDoorActiveState(true);
			AudioManager.Instance.PlayOneShot(doorSlamSound);
		}

		[Button]
		public void OpenDoorway()
		{
			if (!doorwayToTrigger.IsDoorClosed) return;

			doorwayToTrigger?.SetDoorActiveState(false);
			AudioManager.Instance.PlayOneShot(openDoorSound);
		}

		[Button]
		public void CloseDoorway()
		{
			if (doorwayToTrigger.IsDoorClosed) return;

			doorwayToTrigger?.SetDoorActiveState(true);
			AudioManager.Instance.PlayOneShot(closedDoorSound);
		}
	}

	[Serializable]
	public class DoorTriggerEvent
	{
		public ChoreDataSO ChoreRequiredForActivation;
		public StateToSet DoorActionToDo;
		public bool WillTriggerImmediately;

		public string ID => ChoreRequiredForActivation.ID;
	}

	public enum StateToSet
	{
		Open,
		Close,
		Slam
	}
}