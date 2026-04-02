using System;

namespace Unity.Services.Apis.Sample
{
    interface ISectionView : IEditorView
    {
        Section Section { get; }
        string Url { get; }
        string Info { get; }

        void SetStatus(Status status, string message, Exception exception = null);
    }
}
