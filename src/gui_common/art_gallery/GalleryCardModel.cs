﻿using Godot;

public class GalleryCardModel : GalleryCard
{
    private ImageTask? imageTask;

    private Texture imageLoadingIcon = null!;

    private bool finishedLoadingImage;

    public override void _Ready()
    {
        base._Ready();

        imageLoadingIcon = GD.Load<Texture>("res://assets/textures/gui/bevel/IconGenerating.png");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (finishedLoadingImage)
            return;

        if (imageTask != null)
        {
            if (imageTask.Finished)
            {
                Thumbnail = imageTask.FinalImage;
                finishedLoadingImage = true;
            }

            return;
        }

        imageTask = new ImageTask(new ModelPreview(Asset.ResourcePath, Asset.MeshNodePath!));

        PhotoStudio.Instance.SubmitTask(imageTask);

        Thumbnail = imageLoadingIcon;
    }

    public class ModelPreview : IPhotographable
    {
        private string resourcePath;
        private string meshNodePath;

        public ModelPreview(string resourcePath, string meshNodePath)
        {
            this.resourcePath = resourcePath;
            this.meshNodePath = meshNodePath;
        }

        public string SceneToPhotographPath => resourcePath;

        public void ApplySceneParameters(Spatial instancedScene)
        {
        }

        public float CalculatePhotographDistance(Spatial instancedScene)
        {
            var instancedMesh = instancedScene.GetNode<MeshInstance>(meshNodePath);
            return instancedMesh.GetTransformedAabb().Size.Length();
        }
    }
}
