using System.Collections;

class Network : IEnumerable<Layer>
{
    public List<Layer> layers;
    private int position = -1;

    public Network(int nInputs, int nHidden, int nOutputs)
    {
        layers = new List<Layer>();
        Layer hiddenLayer = new Layer(Utils.GenerateList(nHidden, (_) => {
            return new Neuron(Utils.GenerateList(nInputs, (_) => {
                return Utils.Random.NextDouble();
            }), Utils.Random.NextDouble());
        }));
        layers.Add(hiddenLayer);
        Layer outputLayer = new Layer(Utils.GenerateList(nOutputs, (_) => {
            return new Neuron(Utils.GenerateList(nHidden, (_) => {
                return Utils.Random.NextDouble();
            }), Utils.Random.NextDouble());
        }));
        layers.Add(outputLayer);
    }

    public IEnumerator<Layer> GetEnumerator()
    {
        return new NetworkEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new NetworkEnumerator(this);
    }

    private class NetworkEnumerator : IEnumerator<Layer>
    {
        private int position = -1;
        private Network network;
        public NetworkEnumerator(Network network)
        {
            this.network = network;
        }

        public Layer Current => network.layers[position];

        object IEnumerator.Current => network.layers[position];

        public bool MoveNext()
        {
            position++;
            return position < network.layers.Count;
        }

        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
        }
    }
}