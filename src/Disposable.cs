namespace Pain
{
    public class Disposable : IDisposable
    {
        private readonly Action _disposer;

        public Disposable(Action disposer)
        {
            _disposer = disposer;
        }

        public void Dispose()
        {
            _disposer?.Invoke();
        }
    }
}