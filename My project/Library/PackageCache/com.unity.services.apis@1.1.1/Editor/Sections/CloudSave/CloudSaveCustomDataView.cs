using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class CloudSaveCustomDataView : EditorView
    {
        CloudSaveController CloudSave => App.CloudSave;

        readonly CloudSaveSectionView SectionView;


        public CloudSaveCustomDataView(App app, CloudSaveSectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
        }
    }
}
