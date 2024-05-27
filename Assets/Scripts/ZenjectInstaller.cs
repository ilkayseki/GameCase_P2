using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PieceController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ColorManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ObjectPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PathManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AnimatorController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CameraController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MusicManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();

        //Container.BindFactory<Cell, Cell.Factory>().FromComponentInNewPrefab(cellPrefab); // Cell prefab'ını bağlayın
    }


}