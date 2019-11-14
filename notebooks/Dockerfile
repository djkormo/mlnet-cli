# based on https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-current
from ubuntu:18.04
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get update && apt-get install -y wget
RUN wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
#RUN add-apt-repository universe
RUN apt-get update && apt-get install apt-transport-https
RUN apt-get update && apt-get install dotnet-sdk-3.0 -y

RUN dotnet tool install -g mlnet
RUN dotnet tool install dotnet-suggest -g
RUN dotnet tool install -g dotnet-try

RUN apt-get update && apt-get install -y --fix-missing python3-pip

RUN update-alternatives --install /usr/bin/pip pip /usr/bin/pip3 1
RUN update-alternatives --install /usr/bin/python python /usr/bin/python3 1

RUN pip3 install --no-cache-dir ipykernel jupyter notebook jupyterlab

RUN export DIR=$HOME/.dotnet/tools
RUN $DIR/dotnet-try jupyter install 

#RUN dotnet try jupyter install

#RUN jupyter serverextension enable --py jupyterlab --sys-prefix
EXPOSE 8888
CMD ["bash","-c","jupyter lab --ip=0.0.0.0 --allow-root --NotebookApp.token='' --no-browser "]
#CMD ["bash"]