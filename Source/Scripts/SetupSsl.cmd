@echo off

netsh http delete urlacl url=https://+:44301/Design_Time_Addresses/
netsh http add urlacl url=https://+:44301/Design_Time_Addresses/ sddl=D:(A;;GX;;;IU)

netsh http delete urlacl url=https://+:44302/Design_Time_Addresses/
netsh http add urlacl url=https://+:44302/Design_Time_Addresses/ sddl=D:(A;;GX;;;IU)

netsh http delete sslcert ipport=0.0.0.0:44301
netsh http add sslcert ipport=0.0.0.0:44301 certhash=5d36b8eb140fe3eef228b061463f177fd13ac3e8 appid={214124cd-d05b-4309-9af9-9caa44b2b74a} certstorename=MY clientcertnegotiation=enable

netsh http delete sslcert ipport=0.0.0.0:44302
netsh http add sslcert ipport=0.0.0.0:44302 certhash=5d36b8eb140fe3eef228b061463f177fd13ac3e8 appid={214124cd-d05b-4309-9af9-9caa44b2b74a} certstorename=MY clientcertnegotiation=enable
