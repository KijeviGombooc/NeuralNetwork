using System.Collections;

class Neuron : IEnumerable<double>
{
    public double bias;
    public double output = 0;
    public double dEtotal_dOutput = 0;
    public double dOutput_dNet = 0;
    private List<double> weights;
    private List<double> cachedWeights;

    public Neuron(List<double> weights, double bias)
    {
        this.weights = weights;
        this.cachedWeights = Utils.GenerateList(weights.Count(), (_) => 0.0);
        this.bias = bias;
    }

    public void Activate(List<double> inputs)
    {
        output = bias;
        for (int i = 0; i < weights.Count; i++)
            output += weights[i] * inputs[i];
    }

    public void Transfer()
    {
        output = 1.0 / (1.0 + Math.Exp(-output));
    }

    public IEnumerator<double> GetEnumerator()
    {
        return new NeuronEnumerator(this);
    }

    public double this[int index]
    {
        get => weights[index];
        set => weights[index] = value;
    }

    public void SetCachedWeight(int index, double value)
    {
        cachedWeights[index] = value;
    }

    public void UpdateWeights()
    {
        var tmp = this.weights;
        this.weights = this.cachedWeights;
        this.cachedWeights = tmp;
    }

    public override string ToString()
    {
        string weightsString = "";
        for (int i = 0; i < weights.Count - 1; i++)
        {
            weightsString += weights[i] + "; ";
        }
        weightsString += weights[weights.Count - 1];
        return $"{{output: {output}; weights: [{weightsString}]; bias: {bias}}}";
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