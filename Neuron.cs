using System.Collections;

class Neuron : IEnumerable<double>
{
    public double bias;
    public double output = 0;
    private List<double> weights;

    public Neuron(List<double> weights, double bias)
    {
        this.weights = weights;
        this.bias = bias;
    }

    public IEnumerator<double> GetEnumerator()
    {
        return new NeuronEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return  new NeuronEnumerator(this);
    }

    private class NeuronEnumerator : IEnumerator<double>
    {
        private int position = -1;
        private Neuron neuron;
        public NeuronEnumerator(Neuron neuron)
        {
            this.neuron = neuron;
        }

        public double Current => neuron.weights[position];

        object IEnumerator.Current => neuron.weights[position];

        public bool MoveNext()
        {
            position++;
            return position < neuron.weights.Count;
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