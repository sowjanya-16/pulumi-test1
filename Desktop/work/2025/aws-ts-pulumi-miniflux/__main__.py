import pulumi
import pulumi_aws as aws

# Create a VPC
vpc = aws.ec2.Vpc("my-vpc",
    cidr_block="10.0.0.0/16",
    enable_dns_hostnames=True,
    enable_dns_support=True)

# Create an Internet Gateway
igw = aws.ec2.InternetGateway("my-igw",
    vpc_id=vpc.id)

# Create a Route Table and associate it with the VPC
route_table = aws.ec2.RouteTable("my-route-table",
    vpc_id=vpc.id,
    routes=[
        aws.ec2.RouteTableRouteArgs(
            cidr_block="0.0.0.0/0",
            gateway_id=igw.id,
        )
    ])

# Create a public subnet
public_subnet = aws.ec2.Subnet("my-public-subnet",
    vpc_id=vpc.id,
    cidr_block="10.0.1.0/24",
    map_public_ip_on_launch=True)

# Associate the route table with the public subnet
route_table_association = aws.ec2.RouteTableAssociation("my-route-table-association",
    subnet_id=public_subnet.id,
    route_table_id=route_table.id)

# Create a security group that allows SSH from anywhere (Vulnerable)
ssh_security_group = aws.ec2.SecurityGroup("my-vulnerable-ssh-sg",
    vpc_id=vpc.id,
    description="Allow SSH inbound traffic from anywhere",
    ingress=[
        aws.ec2.SecurityGroupIngressArgs(
            protocol="tcp",
            from_port=22,
            to_port=22,
            cidr_blocks=["0.0.0.0/0"], # <-- This is the vulnerability
            description="Allow SSH from everywhere"
        ),
        # Add a placeholder for other traffic to make it a bit more realistic
        aws.ec2.SecurityGroupIngressArgs(
            protocol="tcp",
            from_port=80,
            to_port=80,
            cidr_blocks=["0.0.0.0/0"],
            description="Allow HTTP from everywhere"
        )
    ],
    egress=[
        aws.ec2.SecurityGroupEgressArgs(
            protocol="-1",
            from_port=0,
            to_port=0,
            cidr_blocks=["0.0.0.0/0"],
        )
    ],
    tags={
        "Environment": "dev",
        "Purpose": "Vulnerability_Test_Open_SSH",
    })

# Create an EC2 instance to associate with the vulnerable security group
ami_id = "ami-0c55b159cbfafe1f0" # Example: Amazon Linux 2 AMI (change if needed for your region)
instance = aws.ec2.Instance("my-vulnerable-ec2-instance",
    ami=ami_id,
    instance_type="t2.micro",
    vpc_security_group_ids=[ssh_security_group.id],
    subnet_id=public_subnet.id,
    tags={
        "Name": "VulnerableEC2",
    })

pulumi.export("security_group_id", ssh_security_group.id)
pulumi.export("instance_public_ip", instance.public_ip)
