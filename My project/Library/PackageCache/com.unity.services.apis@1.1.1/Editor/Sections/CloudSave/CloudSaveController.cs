using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Apis.CloudSave;

namespace Unity.Services.Apis.Sample
{
    class CloudSaveController : ApiController
    {
        ITrustedClient TrustedClient { get; set; }
        ICloudSaveDataApi DataApi => TrustedClient.CloudSaveData;
        ICloudSaveFilesApi FilesApi => TrustedClient.CloudSaveFiles;

        public event Action DataChanged;
        public event Action LocalChanged;

        public CloudSaveDataType DataType { get; private set; }
        public CloudSavePlayerDataType PlayerDataType { get; private set; }
        public CloudSaveCustomDataType CustomDataType { get; private set; }
        public string DataIdentifier { get; private set; }

        public List<Item> CloudValues { get; private set; }

        public List<string> AllFiles { get; } = new List<string>();
        public string SelectedFile { get; private set; }

        public CloudSaveController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(ITrustedClient trustedClient)
        {
            TrustedClient = trustedClient;
        }

        public void LoadFileAsync(string file)
        {
            AllFiles.Add(file);
            SelectFile(file);
        }

        public void SelectFile(string file)
        {
            SelectedFile = file;
        }

        public void CreateFileAsync(string file)
        {
            AllFiles.Add(file);
            SelectFile(file);
        }

        public void SetDataType(CloudSaveDataType dataType)
        {
            DataType = dataType;
            Reset();
        }

        public async void SetCustomDataType(CloudSaveCustomDataType dataType)
        {
            CustomDataType = dataType;
            ResetData();

            try
            {
                await LoadCloudSaveValuesAsync();
            }
            catch (Exception)
            { }
        }

        public async void SetPlayerDataType(CloudSavePlayerDataType dataType)
        {
            PlayerDataType = dataType;
            ResetData();

            try
            {
                await LoadCloudSaveValuesAsync();
            }
            catch (Exception)
            { }
        }

        public async void SetIdentifier(string id)
        {
            Reset();
            DataIdentifier = id;

            try
            {
                await LoadCloudSaveValuesAsync();
            }
            catch (Exception)
            { }
        }

        public void Reset()
        {
            DataIdentifier = null;
            ResetData();
        }

        void ResetData()
        {
            CloudValues = new List<Item>();
            DataChanged?.Invoke();
            LocalChanged?.Invoke();
        }

        public async Task LoadCloudSaveValuesAsync()
        {
            await AuthorizeAsync();

            switch (DataType)
            {
                case CloudSaveDataType.Custom:
                    await LoadCustomDataAsync();
                    break;
                case CloudSaveDataType.Player:
                    await LoadPlayerDataAsync();
                    break;
            }
        }

        async Task LoadCustomDataAsync()
        {
            switch (CustomDataType)
            {
                case CloudSaveCustomDataType.Default:
                {
                    var response = await DataApi.GetCustomItems(CloudProjectId, DataIdentifier);
                    response.EnsureSuccessful();
                    CloudValues = response.Data.Results;
                    break;
                }

                case CloudSaveCustomDataType.Private:
                {
                    var response = await DataApi.GetPrivateCustomItems(CloudProjectId, DataIdentifier);
                    response.EnsureSuccessful();
                    CloudValues = response.Data.Results;
                    break;
                }
            }

            DataChanged?.Invoke();
        }

        async Task LoadPlayerDataAsync()
        {
            switch (PlayerDataType)
            {
                case CloudSavePlayerDataType.Default:
                {
                    var response = await DataApi.GetItems(CloudProjectId, DataIdentifier);
                    response.EnsureSuccessful();
                    CloudValues = response.Data.Results;
                    break;
                }

                case CloudSavePlayerDataType.Protected:
                {
                    var response = await DataApi.GetProtectedItems(CloudProjectId, DataIdentifier);
                    response.EnsureSuccessful();
                    CloudValues = response.Data.Results;
                    break;
                }

                case CloudSavePlayerDataType.Public:
                {
                    var response = await DataApi.GetPublicItems(CloudProjectId, DataIdentifier);
                    response.EnsureSuccessful();
                    CloudValues = response.Data.Results;
                    break;
                }
            }

            DataChanged?.Invoke();
        }

        public async Task SetDataAsync(string key, string value)
        {
            await AuthorizeAsync();

            switch (DataType)
            {
                case CloudSaveDataType.Custom:
                    await SetCustomDataAsync(key, value);
                    break;
                case CloudSaveDataType.Player:
                    await SetPlayerDataAsync(key, value);
                    break;
            }
        }

        async Task SetCustomDataAsync(string key, string value)
        {
            switch (CustomDataType)
            {
                case CloudSaveCustomDataType.Default:
                {
                    await DataApi.SetCustomItem(CloudProjectId, DataIdentifier, new SetItemBody(key, value));
                    break;
                }

                case CloudSaveCustomDataType.Private:
                {
                    await DataApi.SetPrivateCustomItem(CloudProjectId, DataIdentifier, new SetItemBody(key, value));
                    break;
                }
            }

            await LoadCloudSaveValuesAsync();
        }

        async Task SetPlayerDataAsync(string key, string value)
        {
            switch (PlayerDataType)
            {
                case CloudSavePlayerDataType.Default:
                {
                    await DataApi.SetItem(CloudProjectId, DataIdentifier, new SetItemBody(key, value));
                    break;
                }

                case CloudSavePlayerDataType.Protected:
                {
                    await DataApi.SetProtectedItem(CloudProjectId, DataIdentifier, new SetItemBody(key, value));
                    break;
                }

                case CloudSavePlayerDataType.Public:
                {
                    await DataApi.SetPublicItem(CloudProjectId, DataIdentifier, new SetItemBody(key, value));
                    break;
                }
            }

            await LoadCloudSaveValuesAsync();
        }

        public async Task DeleteCloudValueAsync(string key)
        {
            await AuthorizeAsync();

            switch (DataType)
            {
                case CloudSaveDataType.Custom:
                    await DeleteCustomDataAsync(key);
                    break;
                case CloudSaveDataType.Player:
                    await DeletePlayerDataAsync(key);
                    break;
            }
        }

        async Task DeleteCustomDataAsync(string key)
        {
            switch (CustomDataType)
            {
                case CloudSaveCustomDataType.Default:
                {
                    await DataApi.DeleteCustomItem(key, CloudProjectId, DataIdentifier);
                    CloudValues.RemoveAll((x => x.Key == key));
                    break;
                }

                case CloudSaveCustomDataType.Private:
                {
                    await DataApi.DeletePrivateCustomItem(key, CloudProjectId, DataIdentifier);
                    CloudValues.RemoveAll((x => x.Key == key));
                    break;
                }
            }

            DataChanged?.Invoke();
        }

        async Task DeletePlayerDataAsync(string key)
        {
            switch (PlayerDataType)
            {
                case CloudSavePlayerDataType.Default:
                {
                    await DataApi.DeleteItem(key, CloudProjectId, DataIdentifier);
                    CloudValues.RemoveAll((x => x.Key == key));
                    break;
                }

                case CloudSavePlayerDataType.Protected:
                {
                    await DataApi.DeleteProtectedItem(key, CloudProjectId, DataIdentifier);
                    CloudValues.RemoveAll((x => x.Key == key));
                    break;
                }

                case CloudSavePlayerDataType.Public:
                {
                    await DataApi.DeletePublicItem(key, CloudProjectId, DataIdentifier);
                    CloudValues.RemoveAll((x => x.Key == key));
                    break;
                }
            }

            DataChanged?.Invoke();
        }

        async Task AuthorizeAsync()
        {
            if (TrustedClient.AccessToken == null)
            {
                await TrustedClient.SignInWithServiceAccount(CloudProjectId, EnvironmentId);
            }
        }
    }
}
