using System;
using Interaction.InteractionCore;
using Utilities;

namespace Interaction
{
	public static class InteractionEvents
	{
		public static event Action<InteractObjectSO> OnItemCollected;
		public static event Action<SerializableGuid> OnInteractEvent;

		public static void RaiseItemCollected(InteractObjectSO item) => OnItemCollected?.Invoke(item);
		public static void RaiseInteractEvent(SerializableGuid id) => OnInteractEvent?.Invoke(id);
	}
}