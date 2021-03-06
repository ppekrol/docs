# Clustering - Overview
In RavenDB 3.5 we introduced clustering as a new major feature.   
In earlier versions of RavenDB, clusters were ad hoc. You created them by bridging together multiple nodes using replication.    

A RavenDB cluster is using Raft (more specifically, [Rachis](./what-is-rachis), our [Raft](https://raft.github.io/) implementation) to 
bridge together multiple servers into a single distributed consensus. A distributed consensus algorithm allows a group of servers to 
agree on the order of a set of commands to execute.   

We use Raft to connect servers together, but what does it mean? RavenDB replication is still running in a multi master mode, which means 
that each server can still accept requests. We use Raft for two distinct purposes.   

The first reason is to distribute changes of operations across the cluster (like adding a new database or changing configuration settings). 
This allows us to be sure that such changes are accepted by the cluster nodes before they are actually performed.   

The second (and more important) reason is that Raft is a strong leader system. We use it to make sure that all the writes in the cluster 
are done through the leader. If the leader is taken down, a new leader is selected transparently, clients will be informed about it, and 
all new writes will go to the new leader. When the previous leader recovers, it will join the cluster as a follower, and there will be 
no disruption of service.   

Note that while the leader selection is handled via Raft, the leader database is replicating to the other nodes using multi-master system, 
so if you need to wait until a document is present in a majority of the cluster, you need to wait for write assurance.   

The stable leader in the presence of failure means that we won't have the clients switching back and forth between nodes, they will be 
informed of the leader, and stick to it. This generates a much more stable network environment, and allowing you to configure the cluster 
details once and have it propagate everywhere is a great reduction in operational work.   

## Related articles

- [Clustering: Rachis - RavenDB's Raft implementation](./what-is-rachis)
- [Studio: Manage Your Server : Clustering](../../../studio/management/cluster)
