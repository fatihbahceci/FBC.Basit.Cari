using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Collections.Concurrent;

namespace FBC.Basit.Cari.Auth
{
    public class FBCCircuitHandlerService : CircuitHandler
    {
        //public ConcurrentDictionary<string, Circuit> Circuits{get;set;}
        //public event EventHandler CircuitsChanged;
        //protected virtual void OnCircuitsChanged()
        //     => CircuitsChanged?.Invoke(this, EventArgs.Empty);

        public FBCCircuitHandlerService()
        {
            //Circuits = new ConcurrentDictionary<string, Circuit>();
        }

        public override Task OnCircuitOpenedAsync(Circuit circuit,
                             CancellationToken cancellationToken)
        {
            //Circuits[circuit.Id] = circuit;
            FBCSessionManager.CirciuitOpened(circuit.Id);
            //OnCircuitsChanged();
            return base.OnCircuitOpenedAsync(circuit, cancellationToken);
        }

        public override Task OnCircuitClosedAsync(Circuit circuit,
                  CancellationToken cancellationToken)
        {
            //Circuits.TryRemove(circuit.Id, out var circuitRemoved);
            FBCSessionManager.CirciuitClosed(circuit.Id);
            //OnCircuitsChanged();
            return base.OnCircuitClosedAsync(circuit, cancellationToken);
        }

        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            return base.OnConnectionDownAsync(circuit, cancellationToken);
        }

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            return base.OnConnectionUpAsync(circuit, cancellationToken);
        }
    }
}