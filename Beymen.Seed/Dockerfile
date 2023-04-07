FROM mongo

COPY init.json /init.json
CMD mongoimport --host mongo --port 27017 --db ConfigurationDatabase --collection Configurations --mode upsert --type json --file init.json --legacy --jsonArray