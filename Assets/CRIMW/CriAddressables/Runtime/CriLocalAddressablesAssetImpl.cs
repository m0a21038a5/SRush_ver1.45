﻿/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ADDRESSABLES_INTEGRATION
 * @{
 */

#if CRI_USE_ADDRESSABLES

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CriWare.Assets
{
	/**
	 * <summary>Addressables 配置の実データ格納先 (Local)</summary>
	 */
	public class CriLocalAddressablesAssetImpl : ICriFileAssetImpl
	{
		public CriLocalAddressablesAssetImpl(CriLocalAddressablesAnchor anchor,  string relativePath, long size, string originalId)
		{
			_anchor = anchor;
			_path = relativePath;
			Size = size;
			_originalId = originalId;
		}

		[SerializeField]
		internal CriLocalAddressablesAnchor _anchor;
		[SerializeField]
		string _path;
		[SerializeField]
		string _originalId;

		public string Path
		{
#if UNITY_EDITOR
			get => _anchor == null ?
				System.IO.Path.GetFullPath(System.IO.Path.Combine(Addressables.ResolveInternalId(CriAddressables.LocalLoadPath), _path)) :
				System.IO.Path.GetFullPath(UnityEditor.AssetDatabase.GUIDToAssetPath(_originalId));
#else
			get
			{
#if UNITY_ANDROID
				/* Replace consecutive '/' in Application.streamingAssetsPath. */
				var streamingAssetsPath = System.Text.RegularExpressions.Regex.Replace(UnityEngine.Application.streamingAssetsPath, "/{2,}", "/");
				/* Exclude streamingAssetsPath from the full path and convert to a path for CRIWARE.   */
				var relativePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Addressables.ResolveInternalId(CriAddressables.LocalLoadPath), _path)).Replace(streamingAssetsPath, string.Empty);
				while (relativePath[0] == '/')
				{
					relativePath = relativePath.Remove(0, 1);
				}
				return relativePath;
#else
				return System.IO.Path.GetFullPath(System.IO.Path.Combine(Addressables.ResolveInternalId(CriAddressables.LocalLoadPath), _path));
#endif
			}
#endif
		}

		internal string InternalPath => _path;

		public ulong Offset => 0;

		[field: SerializeField]
		public long Size { get; private set; }

#if true
		public bool IsReady => true;

		public void OnDisable() { }

		public void OnEnable() { }
#endif
	}
}

#endif

/** @} */
