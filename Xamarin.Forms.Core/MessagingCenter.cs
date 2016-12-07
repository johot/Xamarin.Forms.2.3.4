using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Forms
{
	public static class MessagingCenter
	{
		class Sender : Tuple<string, Type, Type>
		{
			public Sender(string message, Type senderType, Type argType) : base(message, senderType, argType)
			{
			}

			public string Message => Item1;
			public Type SenderType => Item2;
			public Type ArgType => Item3;
		}

		delegate void Callback(object sender, object args);

		class Subscription : Tuple<WeakReference, WeakReference<Callback>>
		{
			protected Subscription(WeakReference subscriber, WeakReference<Callback> callback) 
				: base(subscriber, callback)
			{
			}

			public Subscription(object subscriber, Callback callback) 
				: this(new WeakReference(subscriber), new WeakReference<Callback>(callback))
			{
			}

			public WeakReference Subscriber => Item1;
			public WeakReference<Callback> Callback => Item2;
		}

		static readonly Dictionary<Sender, List<Subscription>> s_subscriptions =
			new Dictionary<Sender, List<Subscription>>();

		public static void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class
		{
			if (sender == null)
				throw new ArgumentNullException(nameof(sender));
			InnerSend(message, typeof(TSender), typeof(TArgs), sender, args);
		}

		public static void Send<TSender>(TSender sender, string message) where TSender : class
		{
			if (sender == null)
				throw new ArgumentNullException(nameof(sender));
			InnerSend(message, typeof(TSender), null, sender, null);
		}

		public static void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback, TSender source = null) where TSender : class
		{
			if (subscriber == null)
				throw new ArgumentNullException(nameof(subscriber));
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));

			Callback wrap = (sender, args) =>
			{
				var send = (TSender)sender;
				if (source == null || send == source)
					callback((TSender)sender, (TArgs)args);
			};

			InnerSubscribe(subscriber, message, typeof(TSender), typeof(TArgs), wrap);
		}

		public static void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = null) where TSender : class
		{
			if (subscriber == null)
				throw new ArgumentNullException(nameof(subscriber));
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));

			Callback wrap = (sender, args) =>
			{
				var send = (TSender)sender;
				if (source == null || send == source)
					callback((TSender)sender);
			};

			InnerSubscribe(subscriber, message, typeof(TSender), null, wrap);
		}

		public static void Unsubscribe<TSender, TArgs>(object subscriber, string message) where TSender : class
		{
			InnerUnsubscribe(message, typeof(TSender), typeof(TArgs), subscriber);
		}

		public static void Unsubscribe<TSender>(object subscriber, string message) where TSender : class
		{
			InnerUnsubscribe(message, typeof(TSender), null, subscriber);
		}

		internal static void ClearSubscribers()
		{
			s_subscriptions.Clear();
		}

		static void InnerSend(string message, Type senderType, Type argType, object sender, object args)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));
			var key = new Sender(message, senderType, argType);
			if (!s_subscriptions.ContainsKey(key))
				return;
			List<Subscription> subcriptions = s_subscriptions[key];
			if (subcriptions == null || !subcriptions.Any())
				return; // should not be reachable

			// ok so this code looks a bit funky but here is the gist of the problem. It is possible that in the course
			// of executing the callbacks for this message someone will subscribe/unsubscribe from the same message in
			// the callback. This would invalidate the enumerator. To work around this we make a copy. However if you unsubscribe 
			// from a message you can fairly reasonably expect that you will therefor not receive a call. To fix this we then
			// check that the item we are about to send the message to actually exists in the live list.
			List<Subscription> subscriptionsCopy = subcriptions.ToList();
			foreach (Subscription subscription in subscriptionsCopy)
			{
				if (subscription.Subscriber.Target != null && subcriptions.Contains(subscription))
				{
					Callback callback;
					if(subscription.Callback.TryGetTarget(out callback))
					{
						callback(sender, args);						
					}
				}
			}
		}

		static void InnerSubscribe(object subscriber, string message, Type senderType, Type argType, Callback callback)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));
			var key = new Sender(message, senderType, argType);
			var value = new Subscription(subscriber, callback);
			if (s_subscriptions.ContainsKey(key))
			{
				s_subscriptions[key].Add(value);
			}
			else
			{
				var list = new List<Subscription> { value };
				s_subscriptions[key] = list;
			}
		}

		static void InnerUnsubscribe(string message, Type senderType, Type argType, object subscriber)
		{
			if (subscriber == null)
				throw new ArgumentNullException(nameof(subscriber));
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			var key = new Sender(message, senderType, argType);
			if (!s_subscriptions.ContainsKey(key))
				return;
			s_subscriptions[key].RemoveAll(tuple => !tuple.Subscriber.IsAlive || tuple.Subscriber.Target == subscriber);
			if (!s_subscriptions[key].Any())
				s_subscriptions.Remove(key);
		}
	}
}