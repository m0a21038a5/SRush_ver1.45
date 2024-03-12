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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;
using System.Linq;
using UnityEngine.ResourceManagement;
using System.IO;

namespace CriWare.Assets
{
	/**
	 * <summary>CRIアセットのデータサイズ実装クラス</summary>
	 */
	[System.Serializable]
	public class CriLocationSizeData : AssetBundleRequestOptions
	{
		public override long ComputeSize(IResourceLocation location, ResourceManager resourceManager)
		{
#if ENABLE_CACHING
            if (File.Exists(CriAddressables.ResourceLocation2Path(location).local))
                return 0;
#endif
            return Convert.ToInt64(Path.ChangeExtension(location.InternalId, null).Split('=').Last(), 16);
		}
	}

	/**
	 * <summary>CRIアセットの Addressables 向けロケーション情報を表現するクラス</summary>
	 */
	public class CriResourceLocation : IResourceLocation
	{
        int m_HashCode;

        public string InternalId { get; }
		public string ProviderId => typeof(CriResourceProvider).FullName;
        public IList<IResourceLocation> Dependencies => null;
        public bool HasDependencies => false;
        public int DependencyHashCode => 0;
        public object Data { get; }

        public string PrimaryKey { get; }

        public Type ResourceType => typeof(IAssetBundleResource);

        public override string ToString() => InternalId;

        public int Hash(Type t) => (m_HashCode * 31 + t.GetHashCode()) * 31 + DependencyHashCode;

        public CriResourceLocation(IResourceLocation original)
        {
			InternalId = original.InternalId;
            m_HashCode = original.InternalId.GetHashCode() * 31 + ProviderId.GetHashCode();
			PrimaryKey = original.PrimaryKey;
			if (original.Data is AssetBundleRequestOptions)
				Data = JsonUtility.FromJson<CriLocationSizeData>(JsonUtility.ToJson(original.Data));
        }
    }
}

#endif

/** @} */
