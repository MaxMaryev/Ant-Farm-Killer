using Assets.Source.Ant;
using UnityEngine;
using Zenject;

public class AntInstaller : MonoInstaller
{
    [SerializeField] private Ant _antPrefab;
    [SerializeField] private Transform _nest;

    public override void InstallBindings()
    {
        BindAnt();
    }

    private void BindAnt() =>
        Container
            .Bind<Ant>()
            .FromInstance(_antPrefab)
            .AsSingle();
}
